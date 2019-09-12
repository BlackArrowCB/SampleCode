using System;
using System.Reflection;
using System.Xml;

namespace STT_ConsoleApp_Test1
{
    class Program
    {
        public abstract class Animal
        {
            abstract public int Legs();
            abstract public string MakeNoise();

            //public int Legs(int LegCount)//Counter productive
            //{
            //    legs = LegCount;
            //    return legs;
            //}

            //public string MakeNoise(string animalSound)
            //{
            //    makeNoise = animalSound;
            //    return makeNoise;
            //}

            public string ToXML(object Object)//Generic object
            {
                Type animal = Object.GetType();
                MethodInfo[] methodInfo = animal.GetMethods();

                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
                xmlDoc.PrependChild(xmlDec);
                XmlElement elemRoot = xmlDoc.CreateElement("Animal");
                xmlDoc.AppendChild(elemRoot);
                XmlElement elem = null;
                foreach (MethodInfo method in methodInfo)
                {
                    if (method.GetBaseDefinition().DeclaringType != method.DeclaringType)
                    {
                        string InnerText = method.Invoke(this, null).ToString();
                        elem = xmlDoc.CreateElement(method.Name);
                        elem.InnerText = InnerText;
                        elemRoot.AppendChild(elem);
                    }
                    //Console.WriteLine("->{0}", method.Name);        

                }

                return elemRoot.InnerXml.ToString();
            }
        }
        public class Dog : Animal
        {
            override public int Legs()
            {
                return 4;
            }

            override public string MakeNoise()
            {
                return "Woof!";
            }
        }

        public class Duck : Animal
        {
            override public int Legs()
            {
                return 2;
            }

            override public string MakeNoise()
            {
                return "Quack!";
            }
        }

        static void Main(string[] args)
        {
            Duck localDuck = new Duck();
            Dog localDog = new Dog();
            Console.WriteLine(localDuck.ToXML(localDuck));
            Console.WriteLine(localDog.ToXML(localDog));
        }
    }
}
