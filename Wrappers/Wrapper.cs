using System.Windows.Media;

namespace CompanyManagement.Wrappers
{
    public class Wrapper
    {
        public int PercentDone { get; set; }
        public SolidColorBrush BarColor { get; set; }
        public string TooltipString { get; set; }
        public Wrapper()
        {
            PercentDone = 0;
            BarColor = Brushes.Gray;
            TooltipString = string.Empty;
        }
        public virtual void InitializeUI(int percentDone)
        {
            PercentDone = percentDone;
            if (percentDone < 30)
            {
                BarColor = Brushes.Red;
            }
            else if (percentDone < 50)
            {
                BarColor = new SolidColorBrush(Color.FromRgb(255, 172, 0));
            }
            else if (percentDone < 80)
            {
                BarColor = new SolidColorBrush(Color.FromRgb(249, 255, 85));
            }
            else
            {
                BarColor = new SolidColorBrush(Color.FromRgb(0, 255, 25));
            }
            TooltipString = string.Format("{0} % complete", percentDone);
        }
    }
}
