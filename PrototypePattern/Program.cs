using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypePattern
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public abstract class Shape : ICloneable
    {

        private String id;
        protected String type;

        abstract void draw();

        public String getType()
        {
            return type;
        }

        public String getId()
        {
            return id;
        }

        public void setId(String id)
        {
            this.id = id;
        }

        public Object clone()
        {
            Object clone = null;

            try
            {
                clone = this.clone();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return clone;
        }
    }
}
