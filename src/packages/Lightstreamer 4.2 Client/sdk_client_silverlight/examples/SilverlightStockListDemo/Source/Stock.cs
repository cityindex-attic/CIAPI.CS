using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows;

namespace SilverlightDemo
{
    public class Stock : INotifyPropertyChanged
    {

        //StockName
        private HighlightField _StockName = new HighlightField();
        public string StockName     { 
            get{
                return _StockName.value;
            }
            set {
                _StockName.value = value;
            }
        }
        public Brush StockNameColor
        {
            get
            {
                return _StockName.color;
            }
            private set
            {
            }
        }

        //LastPrice
        private HighlightField _LastPrice = new HighlightField();
        public string LastPrice
        {
            get
            {
                return _LastPrice.value;
            }
            set
            {
                _LastPrice.value = value;
            }
        }
        public Brush LastPriceColor
        {
            get
            {
                return _LastPrice.color;
            }
            private set
            {
            }
        }

        //Time
        private HighlightField _Time = new HighlightField();
        public string Time
        {
            get
            {
                return _Time.value;
            }
            set
            {
                _Time.value = value;
            }
        }
        public Brush TimeColor
        {
            get
            {
                return _Time.color;
            }
            private set
            {
            }
        }

        //PctChange
        private HighlightField _PctChange = new HighlightField();
        public string PctChange
        {
            get
            {
                return _PctChange.value;
            }
            set
            {
                _PctChange.value = value;
            }
        }
        public Brush PctChangeColor
        {
            get
            {
                return _PctChange.color;
            }
            private set
            {
            }
        }

        //BidQuantity
        private HighlightField _BidQuantity = new HighlightField();
        public string BidQuantity
        {
            get
            {
                return _BidQuantity.value;
            }
            set
            {
                _BidQuantity.value = value;
            }
        }
        public Brush BidQuantityColor
        {
            get
            {
                return _BidQuantity.color;
            }
            private set
            {
            }
        }

        //Bid
        private HighlightField _Bid = new HighlightField();
        public string Bid
        {
            get
            {
                return _Bid.value;
            }
            set
            {
                _Bid.value = value;
            }
        }
        public Brush BidColor
        {
            get
            {
                return _Bid.color;
            }
            private set
            {
            }
        }

        //Ask
        private HighlightField _Ask = new HighlightField();
        public string Ask
        {
            get
            {
                return _Ask.value;
            }
            set
            {
                _Ask.value = value;
            }
        }
        public Brush AskColor
        {
            get
            {
                return _Ask.color;
            }
            private set
            {
            }
        }

        //AskQuantity
        private HighlightField _AskQuantity = new HighlightField();
        public string AskQuantity
        {
            get
            {
                return _AskQuantity.value;
            }
            set
            {
                _AskQuantity.value = value;
            }
        }
        public Brush AskQuantityColor
        {
            get
            {
                return _AskQuantity.color;
            }
            private set
            {
            }
        }
        
        //Min
        private HighlightField _Min = new HighlightField();
        public string Min
        {
            get
            {
                return _Min.value;
            }
            set
            {
                _Min.value = value;
            }
        }
        public Brush MinColor
        {
            get
            {
                return _Min.color;
            }
            private set
            {
            }
        }

        //Max
        private HighlightField _Max = new HighlightField();
        public string Max
        {
            get
            {
                return _Max.value;
            }
            set
            {
                _Max.value = value;
            }
        }
        public Brush MaxColor
        {
            get
            {
                return _Max.color;
            }
            private set
            {
            }
        }

        //RefPrice
        private HighlightField _RefPrice = new HighlightField();
        public string RefPrice
        {
            get
            {
                return _RefPrice.value;
            }
            set
            {
                _RefPrice.value = value;
            }
        }
        public Brush RefPriceColor
        {
            get
            {
                return _RefPrice.color;
            }
            private set
            {
            }
        }

        //OpenPrice
        private HighlightField _OpenPrice = new HighlightField();
        public string OpenPrice
        {
            get
            {
                return _OpenPrice.value;
            }
            set
            {
                _OpenPrice.value = value;
            }
        }
        public Brush OpenPriceColor
        {
            get
            {
                return _OpenPrice.color;
            }
            private set
            {
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyAllChanged()
        {
            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(null));
                    _StockName.Animate();
                    _LastPrice.Animate();
                    _Time.Animate();
                    _PctChange.Animate();
                    _BidQuantity.Animate();
                    _Bid.Animate();
                    _Ask.Animate();
                    _AskQuantity.Animate();
                    _Min.Animate();
                    _Max.Animate();
                    _RefPrice.Animate();
                    _OpenPrice.Animate();
                }
                catch (InvalidOperationException)
                {
                    // it is probably just closing
                }
            }
        }




        private class HighlightField
        {
            private static Duration time = new Duration(TimeSpan.FromSeconds(0.5));

            private string _value;
            public string value
            {
                get
                {
                    return _value;
                }
                set
                {
                    updated = _value != value;
                    _value = value;
                }

            }
            public Brush color { get; private set; }

            private Boolean updated = false;
            private Storyboard story = new Storyboard();

            public HighlightField()
            {
                color = new SolidColorBrush(Colors.Transparent);

                ColorAnimationUsingKeyFrames animColor = new ColorAnimationUsingKeyFrames();
                animColor.KeyFrames.Add(new LinearColorKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.0), Value = Colors.Yellow });
                animColor.KeyFrames.Add(new LinearColorKeyFrame() { KeyTime = time.TimeSpan, Value = Colors.Transparent });
                animColor.Duration = time;

                story.Children.Add(animColor);
                Storyboard.SetTarget(animColor, this.color);
                Storyboard.SetTargetProperty(animColor, new PropertyPath("(Brush.Color)"));
            }

            public void Animate()
            {
                if (this.updated)
                {
                    this.story.Begin();
                    this.updated = false;
                }
            }


        }
    }


    
}
