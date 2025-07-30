using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;
using System.Xml.Linq;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	public enum ScriptSegments
	{
		Open,
		Close,
		SystemFlag,
		Segments,
		Label,
		Locals,
		Mnemonics,
		Name,

	}

	struct ScriptSegment
	{
		public string CodeBlock;
		public List<string> Args;
	}
	struct Label
	{
		public string Name;
		public int Entry;
		public int Count;
		public bool Sub;
	}



    class ScriptBuilder
    {
		// count is important
		// type 1 = item/flag/dialog / type 2 animation
		static Dictionary<ScriptSegments, string> blocks = new()
		{
			{ ScriptSegments.Open, "{" },
			{ ScriptSegments.Close, "}" },
			{ ScriptSegments.Segments, "\"Segments\": [ARG_0]," },
			{ ScriptSegments.Label, "{\"Label\": \"ARG_0\",\"EntryPoint\": ARG_1,\"Count\": ARG_2}," },
			{ ScriptSegments.Locals, "\"ScriptLocal\": [],\"ScriptLocalValue\": []," },
			{ ScriptSegments.Mnemonics, "\"Mnemonics\": [ARG_0]," },
			{ ScriptSegments.SystemFlag, "\"SystemFlag\":{\"SkipInitialize\": 0,\"KeepPlayerPuppet\": 0,\"KeepBadStatusVisual\": 0,\"RidingVehicle\": 0}," },
			{ ScriptSegments.Name, "\"Name\": \"ARG_0\",\"Title\": {\"main\": \"ARG_1\",\"sub\": \"ARG_2\"}" },
		};

		static Dictionary<string, string> mnemonics = new()
		{
			{ "GetItem", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"GetItem\",\"operands\": {  \"iValues\": [ARG_0,ARG_1,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "UseItem", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"UseItem\",\"operands\": {  \"iValues\": [ARG_0,ARG_1,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "TreasureBox", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"TreasureBox\",\"operands\": {  \"iValues\": [ARG_0,ARG_1,ARG_2,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Msg", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Msg\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Msg2", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Msg\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"NUM\",\"zo2\",\"zo3\",\"zo4\",\"zo5\",\"zo6\",\"zo7\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "MsgFunfare", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"MsgFunfare\",\"operands\": {  \"iValues\": [0,0,84,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Nop", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Nop\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "SetScenarioFlag", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"SetFlag\",\"operands\": {  \"iValues\": [ARG_0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ScenarioFlag1\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "SetTreasureFlag", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"SetFlag\",\"operands\": {  \"iValues\": [ARG_0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"TreasureFlag1\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "SetFlag", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"SetFlag\",\"operands\": {  \"iValues\": [ARG_1,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "SetEntities", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"SetEntities\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Branch", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Branch\",\"operands\": {  \"iValues\": [ARG_1,1,ARG_2,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"＝\",\"imm\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Jump", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Jump\",\"operands\": {  \"iValues\": [ARG_0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Wait", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Wait\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [ARG_0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Exit", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Exit\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "StopBGM", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"StopBGM\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "PlayBGM", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"PlayBGM\",\"operands\": {  \"iValues\": [ARG_0,ARG_1,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "PauseBGM", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"PauseBGM\",\"operands\": {  \"iValues\": [0,ARG_0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "FadeIn", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"FadeIn\",\"operands\": {  \"iValues\": [0,0,0,ARG_1,0,0,0,0], \"rValues\": [ARG_0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "SysCall", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"SysCall\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "EncountBoss", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"EncountBoss\",\"operands\": {  \"iValues\": [ARG_0,ARG_1,ARG_2,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_3\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "Call", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Call\",\"operands\": {  \"iValues\": [ARG_0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
			{ "SetPuppet", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"SetPuppet\",\"operands\": {  \"iValues\": [ARG_0,ARG_1,ARG_2,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 2,\"comment\": \"\"}," },
			{ "ExecPuppet", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"ExecPuppet\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 2,\"comment\": \"\"}," },
			{ "CueAnim", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"CueAnim\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 2,\"comment\": \"\"}," },
			{ "Return", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Return\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 2,\"comment\": \"\"}," },
			{ "Nop2", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Nop\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 2,\"comment\": \"\"}," },
			{ "Exit2", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"Exit\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 2,\"comment\": \"\"}," },
			{ "ChangeScript", "{\"label\": \"ARG_LABEL\",\"mnemonic\": \"ChangeScript\",\"operands\": {  \"iValues\": [0,0,0,0,0,0,0,0], \"rValues\": [0,0,0,0,0,0,0,0], \"sValues\": [\"ARG_0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},\"type\": 1,\"comment\": \"\"}," },
		};

		private List<ScriptSegment> addedSegments;
		private List<Label> segmentLabels;
		private string scriptName;
		private bool loadFromJson = false;
		public ScriptBuilder(string name = "")
		{
			addedSegments = new();
			segmentLabels = new();
			scriptName = name;

			if (scriptName != "")
			{
				loadFromJson = true;
			}
		}
		public ScriptBuilder(List<string> segments, string name = "")
		{
			addedSegments = new();
			segmentLabels = new();
			scriptName = name;

			AddSegment(segments);
		}
		/*
		public void Add(ScriptSegments segment, string arg0 = "", string arg1 = "", string arg2 = "")
		{
			addedMnemonics.Add(segment, arg0, arg1, arg2);
		}*/

		public void AddSegment(List<string> segments)
		{
			var result = GenerateSegment(segments);
			addedSegments = result.segments;
			segmentLabels = result.labels;
			//segmentLabels.Add(result.label);
		}
		public static (List<ScriptSegment> segments, List<Label> labels) GenerateSegment(List<string> segments)
		{
			//string segmentlabel = "";
			var scriptSegments = new List<ScriptSegment>();
			var labels = new List<Label>();

			int segmentCount = 0;
			//int labellength = 0;
			string currentsub = "";
			int currententry = 0;


			foreach (var segment in segments)
			{
				var values = segment.Split(" ");

				if (mnemonics.TryGetValue(values[0], out var scriptseg))
				{
					//var argcount = values.Length - 1;
					var arguments = values.ToList();
					arguments.RemoveAt(0);

					scriptSegments.Add(new ScriptSegment()
					{
						CodeBlock = scriptseg,
						Args = arguments
						//Arg0 = argcount >= 1 ? values[1] : "",
						//Arg1 = argcount >= 2 ? values[2] : "",
						//Arg2 = argcount >= 3 ? values[3] : "",
						//Arg3 = argcount >= 4 ? values[4] : "",
					});

					segmentCount++;
				}
				else if (values[0] == "Sub")
				{
					var label = values[1].Replace(":", "");

					if (labels.TryFind(l => l.Name == label, out var existinglabel))
					{
						if (existinglabel.Entry != segmentCount)
						{
							throw new Exception($"Script Builder: Label {label} already exist.");
						}
					}
					else
					{
						if (currentsub != "")
						{
							labels.Add(new Label() { Name = currentsub, Entry = currententry, Count = segmentCount - currententry, Sub = true });
						}

						currentsub = label;
						currententry = segmentCount;
					}

				}
				else if (values[0].Last() == ':')
				{
					var label = values[0].Replace(":", "");

					if (labels.TryFind(l => l.Name == label, out var existinglabel))
					{
						if (existinglabel.Entry != segmentCount)
						{
							throw new Exception($"Script Builder: Label {label} already exist.");
						}
					}
					else
					{
						labels.Add(new Label() { Name = label, Entry = segmentCount, Sub = false });
					}
				}
				else
				{
					throw new Exception($"ScriptBuilder: Invalid mnemonic: {values[0]}.");
				}
			}

			labels.Add(new Label() { Name = currentsub, Entry = currententry, Count = segmentCount - currententry, Sub = true });

			return (scriptSegments, labels);
		}
		public static string ProcessSegments(List<ScriptSegment> segments, List<Label> labels, string name, string anims)
		{
			string script = "";
			string segmentblock = "";
			string mnemonicsblock = "";
			script += blocks[ScriptSegments.Open];

			int segmentcount = 0;

			foreach (var mnemonic in segments)
			{
				string tempsegment = mnemonic.CodeBlock;
				for (int j = 0; j < mnemonic.Args.Count; j++)
				{
					string currentarg = mnemonic.Args[j];

					if (currentarg.First() == '[')
					{
						var labelref = currentarg.Replace("[", "");
						labelref = labelref.Replace("]", "");

						if (labels.TryFind(l => l.Name == labelref, out var foundlabelref))
						{
							currentarg = $"{foundlabelref.Entry}";
						}
						else
						{
							throw new Exception($"ScriptBuilder: Referenced Label {labelref} doesn't exist.");
						}
					}
					tempsegment = tempsegment.Replace($"ARG_{j}", currentarg);
				}

				string label = "";
				if (labels.TryFind(l => l.Entry == segmentcount, out var foundlabel))
				{
					label = foundlabel.Name;
				}

				tempsegment = tempsegment.Replace("ARG_LABEL", label);
				segmentcount++;

				mnemonicsblock += tempsegment;
			}

			var subs = labels.Where(l => l.Sub).ToList();
				

			foreach (var sub in subs)
			{
				string labelsegment = blocks[ScriptSegments.Label];
				labelsegment = labelsegment.Replace("ARG_0", sub.Name);
				labelsegment = labelsegment.Replace("ARG_1", $"{sub.Entry}");
				labelsegment = labelsegment.Replace("ARG_2", $"{sub.Count}");
				segmentblock += labelsegment;
			}

			mnemonicsblock = mnemonicsblock.TrimEnd(',');
			segmentblock = segmentblock.TrimEnd(',');

			string segmentsblock = blocks[ScriptSegments.Segments];
			segmentsblock = segmentsblock.Replace("ARG_0", segmentblock);
			script += segmentsblock;

			script += blocks[ScriptSegments.Locals];

			string mnemonics = blocks[ScriptSegments.Mnemonics];
			mnemonics = mnemonics.Replace("ARG_0", mnemonicsblock);

			script += mnemonics;
			script += blocks[ScriptSegments.SystemFlag];

			if (anims != "")
			{
				anims = anims.TrimStart('{');
				anims = anims.TrimEnd('{');
				script += anims + ",";
			}

			string names = blocks[ScriptSegments.Name];
			names = names.Replace("ARG_0", name);
			names = names.Replace("ARG_1", "dummy");
			names = names.Replace("ARG_2", "subdummy");

			script += names;
			script += blocks[ScriptSegments.Close];

			return script;

		}
		public static string FromScript(List<string> stringsegments, string name, string animfile = "")
		{
			var segments = GenerateSegment(stringsegments);
			var script = ProcessSegments(segments.segments, segments.labels, name, animfile != "" ? FromJson(animfile) : "");

			return script;
		}
		public static string FromJson(string jsonfile)
		{
			string scriptfile = "";
			var assembly = Assembly.GetExecutingAssembly();
			string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(jsonfile + ".json"));
			using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
			{
				using (StreamReader reader = new StreamReader(logicfile))
				{
					scriptfile = reader.ReadToEnd();
				}
			}

			return scriptfile;
		}
		public string Output(string name)
		{

			if (loadFromJson)
			{
				return FromJson(scriptName);
			}
			else
			{
				return ProcessSegments(addedSegments, segmentLabels, name, "");
			}
		}
	}
}
