using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.AppCompat.App;
using Syncfusion.Android.DataForm;
using Syncfusion.Android.DataForm.Editors;
using Syncfusion.Android.MaskedEdit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaskedTextEditor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        SfDataForm dataForm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            dataForm = new SfDataForm(this);
            dataForm.DataObject = new ExpenseInfo();
            dataForm.SetBackgroundColor(Color.White);
            dataForm.RegisterEditor("CustomMaskEditors", new CustomMaskEditor(dataForm,this));
            dataForm.RegisterEditor("Balance", "CustomMaskEditors");
            // Set our view from the "main" layout resource
            SetContentView(dataForm);
        }
    }

    public class CustomMaskEditor : DataFormEditor<SfMaskedEdit>
    {
        Context context;
        public CustomMaskEditor(SfDataForm dataForm, Context _context) : base(dataForm)
        {
            context = _context;
        }

        protected override SfMaskedEdit OnCreateEditorView()
        {
            return new SfMaskedEdit(context);
        }
        protected override void OnInitializeView(DataFormItem dataFormItem, SfMaskedEdit view)
        {
            view.Mask = @"000\.000\.000\.000";
            view.Value = (string)this.DataForm.ItemManager.GetValue(dataFormItem);
        }
        protected override void OnWireEvents(SfMaskedEdit view)
        {
            view.ValueChanged += View_ValueChanged;
        }

        void View_ValueChanged(object sender, ValueChangedEventArgs eventArgs)
        {
            OnCommitValue(sender as SfMaskedEdit);
            OnValidateValue(sender as SfMaskedEdit);
        }

        protected override void OnUnWireEvents(SfMaskedEdit view)
        {
            view.ValueChanged -= View_ValueChanged;
        }
        protected override bool OnValidateValue(SfMaskedEdit view)
        {
            return this.DataForm.Validate("Balance");
        }
        protected override void OnCommitValue(SfMaskedEdit view)
        {
            var dataFormItemView = view.Parent as DataFormItemView;
            this.DataForm.ItemManager.SetValue(dataFormItemView.DataFormItem, view.Value);
        }
    }

    public class ExpenseInfo : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private string balance = "100";
        public string Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
                RaisePropertyChanged("Balance");
            }
        }

        private string _ItemName = "Education";
        public string ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                _ItemName = value;
            }
        }

        public bool HasErrors
        {
            get
            {
                return false;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            var list = new List<string>();
            if (!propertyName.Equals("Balance"))
                return list;
            else
            {
                // Here, you can validate Mask Editor. 
            }
            return list;
        }

    }
}