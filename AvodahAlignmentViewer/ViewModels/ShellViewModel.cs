using AvodahAlignmentViewer.Models;
using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace AvodahAlignmentViewer.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Member Variables   

        private Alignment _clearAlignment = new();  // Avodah alignment
        private Line[]? _goldStandardAlignment;  // NIV alignment
        private string _currentVerse;

        #endregion //Member Variables


        #region Public Properties

        #endregion //Public Properties


        #region Observable Properties

        private Visibility _progressCircleVisibility = Visibility.Visible;
        public Visibility ProgressCircleVisibility
        {
            get => _progressCircleVisibility;
            set
            {
                _progressCircleVisibility = value;
                NotifyOfPropertyChange(() => ProgressCircleVisibility);
            }
        }


        private string _output = string.Empty;
        // ReSharper disable once MemberCanBePrivate.Global
        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                NotifyOfPropertyChange(() => Output);
            }
        }

        private string _versionNumber;
        public string VersionNumber
        {
            get => "Avodah Alignment Viewer - Version: " + _versionNumber;
            set
            {
                _versionNumber = value;
                NotifyOfPropertyChange(() => VersionNumber);
            }
        }


        private string _verseField;
        public string VerseField
        {
            get => _verseField;
            set
            {
                if (value != _currentVerse)
                {
                    _currentVerse = value;
                    GenerateOutput();
                }
                _verseField = value;
                NotifyOfPropertyChange(() => VerseField);
            }
        }


        #endregion //Observable Properties


        #region Constructor

        public ShellViewModel()
        {
            var thisVersion = Assembly.GetEntryAssembly().GetName().Version;
            _versionNumber = $"{thisVersion.Major}.{thisVersion.Minor}.{thisVersion.Build}.{thisVersion.Revision}";

            Output = "Loading Data";
        }


        protected override async void OnViewLoaded(object view)
        {
            await LoadGoldStandard().ConfigureAwait(false);

            await LoadAvodahAlignment().ConfigureAwait(false);



            // grab the first verse
            if (_clearAlignment.verses.Count > 0)
            {
                _currentVerse = _clearAlignment.verses[0].VerseId;

                GenerateOutput();
            }

            // turn off progress circle
            ProgressCircleVisibility = Visibility.Collapsed;

            base.OnViewLoaded(view);
        }

        #endregion //Constructor


        #region Methods


        /// <summary>
        /// Generate the verse output for display
        /// </summary>
        private void GenerateOutput()
        {
            VerseField = _currentVerse;
            NotifyOfPropertyChange(() => VerseField);

            if (_currentVerse == string.Empty)
            {
                Output = "No Verse Selected";
                return;
            }

            // look for non numeric digits
            // ReSharper disable once UnusedVariable
            if (double.TryParse(_currentVerse, out double numericValue) == false)
            {
                Output = "Invalid Verse Number";
                return;
            }

            var verse = _clearAlignment.verses.FirstOrDefault(x => x.VerseId == _currentVerse);
            if (verse is not null)
            {
                _output = $"Book: {_currentVerse.Substring(0, 2)}   Chapter: {_currentVerse.Substring(2, 3)}   Verse: {_currentVerse.Substring(5, 3)}\n";

                _output += $"Malay Verse Text: {verse.VerseText}\n";

                _output += GetNivVerseText() + "\n\n";


                // loop through the manuscript words
                for (int i = 0; i < verse.Manuscript.Count; i++)
                {
                    var source = verse.Manuscript[i];
                    _output += "-----------------------------------------------------------------------------------\n";
                    _output += $"Lemma: {source.Lemma}   MARBLE Pos: {source.MarblePos} ({source.MarblePosLong})\n";
                    _output += $"Domain: {source.MarbleSemanticDomain}\n";
                    _output += $"Domain Definition: {source.MarbleSemanticDomainDefinition}\n";

                    _output += "(Malay - NIV) mappings:\n";
                    _output += "----------------------------\n";

                    var target = GetTargetWord(verse, i);
                    string niv = source.GoldStandardGlosses;
                    if (niv == "")
                    {
                        niv = "[]";
                    }

                    _output += $"  {niv}   -->  {target}\n";
                    _output += "-----------------------------------------------------------------------------------\n";
                    _output += "\n\n";
                }

                NotifyOfPropertyChange(() => Output);
            }
            else
            {
                Output = "Invalid Verse Number";
            }
        }


        /// <summary>
        /// Returns the target word based on an index
        /// </summary>
        /// <param name="verse"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private string GetTargetWord(Verse verse, int i)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int j = 0; j < verse.Alignments.Count; j++)
            {
                var alignment = verse.Alignments[j];
                if (alignment.Source.Contains(i))
                {
                    var targetIndexes = alignment.Target;
                    string output = string.Empty;
                    foreach (var index in targetIndexes)
                    {
                        output += verse.Translation[index].Text + ", ";
                    }
                    return output.Substring(0, output.Length - 2);
                }
            }

            return "[]";
        }


        /// <summary>
        /// Retrieves the complete verse text for the NIV for a verse
        /// </summary>
        /// <returns></returns>
        private string GetNivVerseText()
        {
            if (_goldStandardAlignment != null)
                foreach (var line in _goldStandardAlignment)
                {
                    var verseIdLong = line.manuscript.words[0].id;
                    var verseId = verseIdLong.ToString().Substring(0, 8);
                    if (verseId == _currentVerse)
                    {
                        // convert the NIV tokens into a sentence
                        var nivTargetArray = line.translation;

                        string verseText = "";
                        foreach (var translationWord in nivTargetArray.words)
                        {
                            verseText += translationWord.text + " ";
                        }

                        return $"NIV Verse Text: {verseText.Trim()}";
                    }
                }


            return $"NIV Verse Text: {String.Empty}";
        }


        /// <summary>
        /// Go back one verse
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void VerseBack()
        {
            try
            {
                long verseId = Convert.ToInt64(_currentVerse) - 1;
                _currentVerse = verseId.ToString().Substring(0, 8);
                GenerateOutput();
            }
            catch (Exception e)
            {
                Output = e.Message;
            }
        }

        /// <summary>
        /// Go forward a verse
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void VerseForward()
        {
            try
            {
                long verseId = Convert.ToInt64(_currentVerse) + 1;
                _currentVerse = verseId.ToString().Substring(0, 8);
                GenerateOutput();
            }
            catch (Exception e)
            {
                Output = e.Message;
            }
        }


        /// <summary>
        /// Load in the avodah alignments and deserialize them into an object
        /// </summary>
        /// <returns></returns>
        private async Task LoadAvodahAlignment()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, @"DataFiles\alignmentNTclear.json");

            if (File.Exists(filePath) == false)
            {
                Output = $"Cannot Find File: {filePath}";
                return;
            }


            var jsonText = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);

            await Task.Run(() =>
            {
                try
                {
#pragma warning disable CS8601
                    _clearAlignment = JsonConvert.DeserializeObject<Alignment>(jsonText);
#pragma warning restore CS8601
                }
                catch (Exception e)
                {
                    OnUIThread(() =>
                    {
                        Output = $"Deserialization Error: {e.Message}";
                    });
                }
            });
        }

        /// <summary>
        /// Load in the NIV and deserialize it into an object
        /// </summary>
        /// <returns></returns>
        private async Task LoadGoldStandard()
        {
            var nivPath = Path.Combine(Environment.CurrentDirectory, @"DataFiles\niv84.nt.alignment.json");

            if (File.Exists(nivPath) == false)
            {
                Output = $"Cannot Find File: {nivPath}";
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    _goldStandardAlignment = JsonConvert.DeserializeObject<Line[]>(File.ReadAllText(nivPath));
                }
                catch (Exception e)
                {
                    OnUIThread(() =>
                    {
                        Output = $"Deserialization Error: {e.Message}";
                    });
                }
            });
        }


        /// <summary>
        /// Exit the program
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void Close()
        {
            this.TryCloseAsync();
        }

        #endregion // Methods

    }
}
