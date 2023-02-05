using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvodahAlignmentViewer.Models
{
    [Serializable]
    public class Alignment
    {
        public List<Verse> verses = new List<Verse>();
    }

    [Serializable]
    public class Verse
    {
        public string VerseId { get; set; } = "";
        public string VerseText { get; set; } = "";
        public string VerseRange { get; set; } = "";
        public EVerseCheckedState CheckedState { get; set; } = EVerseCheckedState.notChecked;
        public List<Source> Manuscript { get; set; } = new List<Source>();
        public List<Target> Translation { get; set; } = new List<Target>();
        public List<LinkGroup> Alignments { get; set; } = new List<LinkGroup>();
        public double LastChanged { get; set; }
    }


    [Serializable]
    public class Source : Common
    {
        public string Strong { get; set; } = "";
        public string Gloss { get; set; } = "";
        public string Gloss2 { get; set; } = "";
        public string Lemma { get; set; } = "";
        public string Pos { get; set; } = "";
        public string Morph { get; set; } = "";
        public string GoldStandardTranslationIds { get; set; } = "";
        public string GoldStandardGlosses { get; set; } = "";
        public string MarbleSemanticDomain { get; set; } = "";
        public string MarblePos { get; set; } = "";
        public string MarblePosLong { get; set; } = "";
        public string MarbleSemanticDomainDefinition { get; set; } = "";
    }

    [Serializable]
    public class Target : Common
    {

    }

    [Serializable]
    public class Common
    {
        public string Id { get; set; } = ""; //now a string from long
        public string AltId { get; set; } = "";
        public string Text { get; set; } = "";
    }

    public enum EVerseCheckedState
    {
        notChecked = 0,
        verified = 1,
        needsReview = 2,
        verifiedProvisional = 3,
        verifiedTeam = 4,
        verifiedConsultant = 5,
    }

    public enum ELinkType
    {
        unknown = 0,
        key = 1,
        morphological = 2,
        assistive = 3,
        substitution = 4,
        dynamic = 5,
    }

    public enum EAlignType
    {
        unknown = 0,
        auto = 1,
        manual = 2,
    }

    public enum EAlignState
    {
        NotSet = -1,
        notChecked = 0,
        verified = 1,
        bad = 2,
        question = 3,
    }

    [Serializable]
    public class LinkGroup
    {
        public List<int> Source { get; set; } = new List<int>();
        public List<int> Target { get; set; } = new List<int>();
        public double Cscore { get; set; } = -1;
        public ELinkType LinkType { get; set; } = ELinkType.unknown;
        public EAlignType AlignType { get; set; } = EAlignType.unknown;
        public EAlignState AlignState { get; set; } = EAlignState.notChecked;
        public int ValidationLevel { get; set; } = 0;
    }
}
