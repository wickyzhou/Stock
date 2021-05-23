using Common;
using Stock.Command;
using Stock.Model;
using Stock.Service;
using Stock.View.InfoWindow.DataAnalysis;
using Stock.ViewModel.InfoWindow.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Controls;

namespace Stock.ViewModel.IndexPage
{
    public class DataAnalysisPageViewModel : BaseViewModel
    {
        private static readonly CustomIndicatorService _customIndicatorService = new CustomIndicatorService();

        public DataAnalysisPageViewModel()
        {

            InitData();
            InitCommand();

        }

        private ObservableCollection<PreferenceStockModel> preferenceStockLists;

        public ObservableCollection<PreferenceStockModel> PreferenceStockLists
        {
            get { return preferenceStockLists; }
            set
            {
                preferenceStockLists = value;
                this.RaisePropertyChanged(nameof(PreferenceStockLists));
            }
        }

        private PreferenceStockModel preferenceStockSelectedItem;

        public PreferenceStockModel PreferenceStockSelectedItem
        {
            get { return preferenceStockSelectedItem; }
            set
            {
                preferenceStockSelectedItem = value;
                this.RaisePropertyChanged(nameof(PreferenceStockSelectedItem));
            }
        }

        private bool isAutoSyncHugeAmount = false;

        public bool IsAutoSyncHugeAmount
        {
            get { return isAutoSyncHugeAmount; }
            set
            {
                isAutoSyncHugeAmount = value;
                this.RaisePropertyChanged(nameof(IsAutoSyncHugeAmount));
            }
        }

        private string codeFilter = "";

        public string CodeFilter
        {
            get { return codeFilter; }
            set
            {
                codeFilter = value;
                this.RaisePropertyChanged(nameof(CodeFilter));
            }
        }

        private ObservableCollection<HugeAmountFocusedStockModel> hugeAmountFocusedStockLists;

        public ObservableCollection<HugeAmountFocusedStockModel> HugeAmountFocusedStockLists
        {
            get { return hugeAmountFocusedStockLists; }
            set
            {
                hugeAmountFocusedStockLists = value;
                this.RaisePropertyChanged(nameof(HugeAmountFocusedStockLists));
            }
        }

        private HugeAmountFocusedStockModel hugeAmountFocusedStockSelectedItem;

        public HugeAmountFocusedStockModel HugeAmountFocusedStockSelectedItem
        {
            get { return hugeAmountFocusedStockSelectedItem; }
            set
            {
                hugeAmountFocusedStockSelectedItem = value;
                this.RaisePropertyChanged(nameof(HugeAmountFocusedStockSelectedItem));
            }
        }



        private ObservableCollection<CustomIndicatorModel> customIndicatorLists;

        public ObservableCollection<CustomIndicatorModel> CustomIndicatorLists
        {
            get { return customIndicatorLists; }
            set
            {
                customIndicatorLists = value;
                this.RaisePropertyChanged(nameof(CustomIndicatorLists));
            }
        }

        private CustomIndicatorModel customIndicatorSelectedItem;

        public CustomIndicatorModel CustomIndicatorSelectedItem
        {
            get { return customIndicatorSelectedItem; }
            set
            {
                customIndicatorSelectedItem = value;
                this.RaisePropertyChanged(nameof(CustomIndicatorSelectedItem));
                this.RaisePropertyChanged(nameof(CustomIndicatorLists));
            }
        }

        private ObservableCollection<StockBaseModel> customIndicatorResultLists;

        public ObservableCollection<StockBaseModel> CustomIndicatorResultLists
        {
            get { return customIndicatorResultLists; }
            set
            {

                customIndicatorResultLists = value;
                this.RaisePropertyChanged(nameof(CustomIndicatorResultLists));
            }
        }

        private StockBaseModel customIndicatorResultSelectedItem;

        public StockBaseModel CustomIndicatorResultSelectedItem
        {
            get { return customIndicatorResultSelectedItem; }
            set
            {
                customIndicatorResultSelectedItem = value;
                this.RaisePropertyChanged(nameof(CustomIndicatorResultSelectedItem));
            }
        }


        public DelegateCommand PreferenceStockQueryCommand { get; set; }
        public DelegateCommand HugeAmountAutoSyncCommand { get; set; }
        public DelegateCommand PreferenceStockSelectionChangedCommand { get; set; }
        public DelegateCommand HugeAmountFocusedStockSelectionChangedCommand { get; set; }
        public DelegateCommand RecommendedStockQueryCommmand { get; set; }
        public DelegateCommand CustomIndicatorAddCommand { get; set; }
        public DelegateCommand CustomIndicatorModifyCommand { get; set; }
        public DelegateCommand CustomIndicatorSelectedAllCommand { get; set; }
        public DelegateCommand CustomIndicatorUnSelectedAllCommand { get; set; }
        public DelegateCommand CustomIndicatorResultSelectionChangedCommand { get; set; }
        public DelegateCommand CustomIndicatorMouseLeftClickCommand { get; set; }



        private void InitCommand()
        {
            PreferenceStockQueryCommand = new DelegateCommand((obj) => { });
            HugeAmountAutoSyncCommand = new DelegateCommand((obj) => { });
            PreferenceStockSelectionChangedCommand = new DelegateCommand((obj) => { });
            HugeAmountFocusedStockSelectionChangedCommand = new DelegateCommand((obj) => { });
            RecommendedStockQueryCommmand = new DelegateCommand((obj) => { });

            CustomIndicatorMouseLeftClickCommand = new DelegateCommand((obj) =>
            {
                CustomIndicatorSelectedItem = (obj as DataGridRow).Item as CustomIndicatorModel;
                CustomIndicatorSelectedItem.IsChecked=!CustomIndicatorSelectedItem.IsChecked;
            });


            CustomIndicatorAddCommand = new DelegateCommand((obj) =>
            {
                //MD5 md5 = MD5.Create(); var inputBytes = Encoding.Unicode.GetBytes("000001PAYH"); var hash = md5.ComputeHash(inputBytes);
                //StringBuilder sb = new StringBuilder();

                CustomIndicatorModel inputEntity = new CustomIndicatorModel();
                CustomIndicatorCreateView view = new CustomIndicatorCreateView();

                (view.DataContext as CustomIndicatorCreateViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();

                    if (type == 1)
                    {
                        outputEntity.Id = _customIndicatorService.GetContinuousId();
                        if (_customIndicatorService.Insert(outputEntity))
                            CustomIndicatorLists.Add(outputEntity);
                    }
                });
                view.ShowDialog();
             });

            CustomIndicatorModifyCommand = new DelegateCommand((obj) =>
            {
                CustomIndicatorCreateView view = new CustomIndicatorCreateView();
                var clone= ObjectDeepCopy<CustomIndicatorModel,CustomIndicatorModel>.Trans(CustomIndicatorSelectedItem);
                (view.DataContext as CustomIndicatorCreateViewModel).WithParam(clone, (type, outputEntity) =>
                {
                    view.Close();

                    if (type == 1)
                    {
                        if (_customIndicatorService.Update(outputEntity))
                            ModelTypeHelper.PropertyMapper(CustomIndicatorSelectedItem,outputEntity);
                    }
                });
                view.ShowDialog();
            });

            CustomIndicatorSelectedAllCommand = new DelegateCommand((obj) => { });
            CustomIndicatorUnSelectedAllCommand = new DelegateCommand((obj) => { });
            CustomIndicatorResultSelectionChangedCommand = new DelegateCommand((obj) => { });
        }

        private void InitData()
        {
            PreferenceStockLists = new ObservableCollection<PreferenceStockModel>();
            PreferenceStockSelectedItem = new PreferenceStockModel();
            HugeAmountFocusedStockLists = new ObservableCollection<HugeAmountFocusedStockModel>();
            HugeAmountFocusedStockSelectedItem = new HugeAmountFocusedStockModel();
            CustomIndicatorLists = new ObservableCollection<CustomIndicatorModel>(_customIndicatorService.GetLists());
            CustomIndicatorSelectedItem = new CustomIndicatorModel();
            CustomIndicatorResultLists = new ObservableCollection<StockBaseModel>();
            CustomIndicatorResultSelectedItem = new StockBaseModel();

        }
    }
}
