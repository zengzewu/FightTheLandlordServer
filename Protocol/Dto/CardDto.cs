using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class CardDto
    {        
        public int Color { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }

        public CardDto()
        {

        }

        public CardDto(int color,int weight,string name)
        {
            this.Color = color;
            this.Weight = weight;
            this.Name = name;
        }
    }

}
