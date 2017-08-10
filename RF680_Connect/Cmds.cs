using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RF680_Connect
{
    public class Math
    {
        public int Add(int Param1, int Param2)
        {
            return Param1 + Param2;
        }

        private int Subtract(int Param1, int Param2)
        {
            return Param1 - Param2;
        }
    }

    public static class Cmds
    {
        public static string HostGreet(int id)
        {
            return @"
            <frame>
                <cmd>
                    <id>" + id + @"</id>
                    <hostGreetings>
                        <readerType>SIMATIC_RF680R</readerType>
                        <supportedVersions>
                            <version>V2.0</version>
                            <version>V2.1</version>
                            <version>V2.2</version>
                        </supportedVersions>
                    </hostGreetings>
                </cmd>
            </frame>";
        }

        public static string readTagIDs(int id)
        {
            return @"
            <frame>
                <cmd>
                    <id>" + id + @"</id>
                    <readTagIDs>
                        <sourceName>Readpoint_1</sourceName>
                        <duration>0</duration>
                        <unit>Time</unit>                      
                    </readTagIDs>
                </cmd>
            </frame>";
        }

        public static string writeTagID(int id, string Text, string tID)
        {
            Byte[] b = ASCIIEncoding.ASCII.GetBytes(Text);

            return @"
            <frame>
	            <cmd>
		            <id>" + id + @"</id>
		            <writeTagID>
			            <sourceName>Readpoint_1</sourceName>
			            <tagID>" + tID + @"</tagID> // opt
			            <newID>" + Text + @"</newID>
		            </writeTagID>
	            </cmd>
            </frame>";
        }
    }




    class Tag
    {
        public string ID = "";
        public string PC = "";

        public override string ToString()
        {

            return this.ID;
        }
    }  
}