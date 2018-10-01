using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;



namespace ConversieTimKud
{
	class Program
    {
        static void Main(string[] args)
        {
			string filePath = @"/Users/hungle/Downloads/moeder3kolom.csv";
            
			List<InktContainer> inktContainers = new List<InktContainer>();
			List<string> lines = File.ReadAllLines(filePath).Skip(1).ToList();
            foreach (var line in lines)
			{
				string[] entries = line.Split(',');

				InktContainer newContainer = new InktContainer();
				newContainer.Printer_merk = entries[0];
				newContainer.Printer_type = entries[1];
				newContainer.Printer_model = entries[2];

				inktContainers.Add(newContainer);
               
			}

			//foreach (var container in inktContainers)
            //{
            //    Console.WriteLine($"{container.Printer_merk} {container.Printer_type} {container.Printer_model}");
            //}

			var queryNestedGroups =
                from container in inktContainers
                group container by container.Printer_merk into newGroup1
                from newGroup2 in
                    (from container in newGroup1
                    group container by container.Printer_type)
                group newGroup2 by newGroup1.Key;

			//foreach (var outerGroup in queryNestedGroups)
			//{
			//    Console.WriteLine($"Merk van Printer = {outerGroup.Key}");
			//    foreach (var innerGroup in outerGroup)
			//    {
			//        Console.WriteLine($"\tType van Printer: {innerGroup.Key}");
			//        foreach (var innerGroupElement in innerGroup)
			//        {
			//            Console.WriteLine($"\t\t{innerGroupElement.Printer_merk} {innerGroupElement.Printer_model}");
			//        }
			//    }
			//}
			int num_maincat = 0;
            
            
                     
            foreach (var merkGroup in queryNestedGroups)
			{
				Console.WriteLine($"Merk van Printer = {merkGroup.Key}");
				num_maincat ++;                
			}
			XElement xml = new XElement("resultlist",
										new XElement("header",
													 new XElement("num_maincat", num_maincat)),
									   new XElement("categories",
													queryNestedGroups.Select(m => new XElement("maincat",
																							 new XAttribute("id", m.Key),
			                                                                                 new XAttribute("name",m.Key),
			                                                                                   new XAttribute("num_subcat",m.Count()),
			                                                             m.Select(t => new XElement("level_1",
			                                                                                        new XAttribute("id",t.Key),
			                                                                                        new XAttribute("name",t.Key),
			                                                                                        new XAttribute("num_subcat", t.Count()),               
                                                                                                    t.Select(l => new XElement("level_2",
			                                                                                                                   new XAttribute("id", l.Printer_model),
			                                                                                                                   new XAttribute("name",l.Printer_model))
			                                                                                                )))))));



			xml.Save("MotherFinal.xml");                                     
			

			Console.WriteLine(xml);
		}
    }
}
