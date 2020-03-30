using System;

namespace XxSlitFrame.View.Button
{
    public enum ButtonMode
    {
        AlwaysEnabled,
        EnabledInPlayMode,
        DisabledInPlayMode
    }

    [Flags]
    public enum ButtonSpacing
    {
        None = 0,
        Before = 1,
        After = 2
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class XButtonAttribute : Attribute
    {
        private string name = null;
        private ButtonMode mode = ButtonMode.AlwaysEnabled;
        private ButtonSpacing spacing = ButtonSpacing.None;

        public string Name
        {
            get { return name; }
        }

        public ButtonMode Mode
        {
            get { return mode; }
        }

        public ButtonSpacing Spacing
        {
            get { return spacing; }
        }

        public XButtonAttribute()
        {
        }

        public XButtonAttribute(string name)
        {
            this.name = name;
        }

        public XButtonAttribute(ButtonMode mode)
        {
            this.mode = mode;
        }

        public XButtonAttribute(ButtonSpacing spacing)
        {
            this.spacing = spacing;
        }

        public XButtonAttribute(string name, ButtonMode mode)
        {
            this.name = name;
            this.mode = mode;
        }

        public XButtonAttribute(string name, ButtonSpacing spacing)
        {
            this.name = name;
            this.spacing = spacing;
        }

        public XButtonAttribute(string name, ButtonMode mode, ButtonSpacing spacing)
        {
            this.name = name;
            this.mode = mode;
            this.spacing = spacing;
        }
    }
}