using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using csas.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Xml;

namespace csas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new Upload());
        }

        [HttpPost]
        public IActionResult CharacterSheet(IFormFile file)
        {
            Character character = new Character();
            List<string> Elements = new List<string>();
            List<string> ElementExceptions = new List<string>();
            ElementExceptions.Add("character");
            ElementExceptions.Add("adventurelist");
            ElementExceptions.Add("temp");

            //Remove From Exceptions when we can.
            //ElementExceptions.Add("backgroundlink"); //Todo Need to store this but not sure where yet. 
            ElementExceptions.Add("featlist");
            //ElementExceptions.Add("powermode");
            ElementExceptions.Add("powers");
            //ElementExceptions.Add("racelink");
            //ElementExceptions.Add("token");


           StringBuilder errorLog = new StringBuilder();

            var result = new StringBuilder();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                XmlReader reader = XmlReader.Create(stream, settings);

                reader.MoveToContent();

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (!ElementExceptions.Contains(reader.Name.ToLower()))
                            {
                                Elements.Add(reader.Name);
                            }
                         
                            break;
                        case XmlNodeType.Text:
                            Console.Write(reader.Value);
                            if (Elements[0].ToLower() == "abilities")
                            {
                                //Grab the ability we are currently working with based of the Elements
                                var currentAbility = character.AbilityScores.FirstOrDefault(x => x.Name.ToString().ToLower() == Elements[1].Substring(0, 3).ToLower());

                                //If the ability does not exisit yet (ie the first attribute of the ability) set it up
                                if (currentAbility == null)
                                {
                                    currentAbility = new AbilityScore();
                                    if (Elements[1].ToLower() == "charisma") { currentAbility.Name = AbilityNames.Cha; }
                                    else if (Elements[1] == "constitution") { currentAbility.Name = AbilityNames.Con; }
                                    else if (Elements[1] == "dexterity") { currentAbility.Name = AbilityNames.Dex; }
                                    else if (Elements[1] == "intelligence") { currentAbility.Name = AbilityNames.Int; }
                                    else if (Elements[1] == "strength") { currentAbility.Name = AbilityNames.Str; }
                                    else if (Elements[1] == "wisdom") { currentAbility.Name = AbilityNames.Wis; }
                                    else { currentAbility.Name = AbilityNames.Custom; currentAbility.CustomName = Elements[1]; }

                                    character.AbilityScores.Add(currentAbility);
                                }

                                //Set the property of the abilit score based of the name of the 3rd Element.
                                if (Elements[2].ToLower() == "bonus") { currentAbility.Bonus = Convert.ToInt32(reader.Value); }
                                else if (Elements[2] == "save") { currentAbility.Save = Convert.ToInt32(reader.Value); }
                                else if (Elements[2] == "savemodifier") { currentAbility.SaveMod = Convert.ToInt32(reader.Value); }
                                else if (Elements[2] == "saveprof") { currentAbility.SaveProf = Convert.ToInt32(reader.Value); }
                                else if (Elements[2] == "score") { currentAbility.Value = Convert.ToInt32(reader.Value); }
                                else { errorLog.AppendLine("Not expecting Element " + Elements[0] + " >> " + Elements[1] + " >> " + Elements[2]); }

                            }
                            else if (Elements[0].ToLower() == "alignment")
                            {
                                if (reader.Value.ToLower() == "lawful good") { character.Details.Alignment = Alignment.LawfulGood; }
                                if (reader.Value.ToLower() == "lawful neutral") { character.Details.Alignment = Alignment.LawfulNeutral; }
                                if (reader.Value.ToLower() == "lawful evil") { character.Details.Alignment = Alignment.LawfulEvil; }
                                if (reader.Value.ToLower() == "neutral good") { character.Details.Alignment = Alignment.NeutralGood; }
                                if (reader.Value.ToLower() == "true neutral") { character.Details.Alignment = Alignment.TrueNeutral; }
                                if (reader.Value.ToLower() == "neutral evil") { character.Details.Alignment = Alignment.NeutralEvil; }
                                if (reader.Value.ToLower() == "chaotic good") { character.Details.Alignment = Alignment.ChaoticGood; }
                                if (reader.Value.ToLower() == "chaotic neutral") { character.Details.Alignment = Alignment.ChaoticNeutral; }
                                if (reader.Value.ToLower() == "chaitic evil") { character.Details.Alignment = Alignment.ChaiticEvil; }
                            }
                            else if (Elements[0].ToLower() == "appearance") { character.Details.Appearance = reader.Value; }
                            else if (Elements[0].ToLower() == "background") { character.Details.Background = reader.Value; }
                            else if (Elements[0].ToLower() == "personalitytraits") { character.Peronality.Traits = reader.Value; }
                            else if (Elements[0].ToLower() == "bonds") { character.Peronality.Bonds = reader.Value; }
                            else if (Elements[0].ToLower() == "flaws") { character.Peronality.Flaws = reader.Value; }
                            else if (Elements[0].ToLower() == "ideals") { character.Peronality.Ideals = reader.Value; }
                            else if (Elements[0].ToLower() == "exp") { character.Details.ExperincePoints = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "expneeded") { character.Details.ExperincePointsNeeded = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "expneeded") { character.Details.ExperincePointsNeeded = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "level") { character.Details.CharacterLevel = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "name") { character.Name = reader.Value; }
                            else if (Elements[0].ToLower() == "perception") { character.Details.Perception = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "perceptionmodifier")  { character.Details.PerceptionMod = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "profbonus") { character.Details.ProficencyBonus = Convert.ToInt32(reader.Value); }
                            else if (Elements[0].ToLower() == "race") { character.Race = reader.Value; }
                            else if (Elements[0].ToLower() == "senses") { character.Details.Senses = reader.Value; }
                            else if (Elements[0].ToLower() == "size") { character.Details.Size = reader.Value; }
                            else if (Elements[0].ToLower() == "skilllist") 
                            {
                                var currentSkill = character.Skills.FirstOrDefault(x => x.SourceId.ToLower() == Elements[1].ToLower());

                                //If the ability does not exisit yet (ie the first attribute of the ability) set it up
                                if (currentSkill == null)
                                {
                                    currentSkill = new Skill();
                                    currentSkill.SourceId = Elements[1];
                                    character.Skills.Add(currentSkill);
                                }

                                if (Elements[2] == "misc")
                                {
                                    currentSkill.MiscBonus = Convert.ToInt32(reader.Value);
                                }
                                else if (Elements[2] == "name") 
                                {
                                    currentSkill.Name = reader.Value;
                                }
                                else if (Elements[2] == "prof")
                                {
                                    currentSkill.ProfBonus = Convert.ToInt32(reader.Value);
                                }
                                else if (Elements[2] == "stat")
                                {
                                    var abilityScore = character.AbilityScores.FirstOrDefault(x => x.Name.ToString().ToLower() == reader.Value.Substring(0, 3).ToLower());
                                    if (abilityScore != null)
                                    {
                                        currentSkill.Ability = abilityScore;
                                    }
                                }
                                else if (Elements[2] == "total")
                                {
                                    currentSkill.Value = Convert.ToInt32(reader.Value);
                                }
                            }
                            break;
                        case XmlNodeType.CDATA:
                            Console.Write("<![CDATA[{0}]]>", reader.Value);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Comment:
                            Console.Write("<!--{0}-->", reader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            Console.Write("<?xml version='1.0'?>");
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.EntityReference:
                            Console.Write(reader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            var currentElement = Elements.FirstOrDefault(x => x == reader.Name);
                            if (currentElement != null)
                            {
                                Elements.Remove(currentElement);
                            }

                            break;
                    }
                }
            }
            return View(character);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
