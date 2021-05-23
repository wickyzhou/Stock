using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MainMenuModel
    {
        public int ID { get; set; }

        public string Uri { get; set; }

        public string TB1Text { get; set; }

        public string TB1FontFamily { get; set; }

        public int TB1FontSize { get; set; }

        public string TB2Text { get; set; }

        public string TB2FontFamily { get; set; }

        public int TB2FontSize { get; set; }

        public int RoleOwner { get; set; }

        public int UserOwner { get; set; }

        public double TB1MarginLeft { get; set; }

        public double TB1MarginTop { get; set; }

        public double TB1MarginRight { get; set; }

        public double TB1MarginBottom { get; set; }

        public double TB2MarginLeft { get; set; }

        public double TB2MarginTop { get; set; }

        public double TB2MarginRight { get; set; }

        public double TB2MarginBottom { get; set; }

    }
}
