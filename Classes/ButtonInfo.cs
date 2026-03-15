using System;

namespace StupidTemplate.Classes
{
    public class ButtonInfo
    {
        public string buttonText = "-"; // no button text = not good!
        public string overlapText = null; // this is used for like one thing
        public Action method = null;
        public Action enableMethod = null; // if isTogglable
        public Action disableMethod = null; // if isTogglable
        public bool enabled = false;
        public bool isTogglable = true;
        public string toolTip = "This button doesn't have a tooltip/tutorial."; // default tooltip, might change this to be nothing
    }
}
