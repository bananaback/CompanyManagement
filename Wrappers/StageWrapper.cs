using CompanyManagement.EF;

namespace CompanyManagement.Wrappers
{
    public class StageWrapper : Wrapper
    {
        private Stage _stage;
        public StageWrapper(Stage stage) : base()
        {
            _stage = stage;
        }
        public string ID => _stage.ID;
        public string Description => _stage.Description;
    }
}
