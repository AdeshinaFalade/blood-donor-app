using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using FR.Ganfra.Materialspinner;
using Google.Android.Material.TextField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace blood_donor_app.Fragments
{
    
    public class NewDonorFragment : DialogFragment
    {
        TextInputLayout txtName;
        TextInputLayout txtCity;
        TextInputLayout txtCountry;
        TextInputLayout txtPhone;
        TextInputLayout txtEmail;
        MaterialSpinner materialSpinner;
        Button saveButton;
        List<string> bloodGroups;
        ArrayAdapter<string> spinnerAdapter;
        private string selectedBloodGroup;

        public event EventHandler<DonorDetailsEventArgs> OnDonorRegistered;

        public class DonorDetailsEventArgs : EventArgs
        {
            public Donor Donor { get; set; }    
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.add_new, container, false);
            ConnectViews(view);
            SetupSpinner();
            
            return view;

        }

        void ConnectViews(View view)
        {
            txtName = view.FindViewById<TextInputLayout>(Resource.Id.txtName);
            txtCity = view.FindViewById<TextInputLayout>(Resource.Id.txtCity);
            txtCountry = view.FindViewById<TextInputLayout>(Resource.Id.txtCountry);
            txtPhone = view.FindViewById<TextInputLayout>(Resource.Id.txtPhone);
            txtEmail = view.FindViewById<TextInputLayout>(Resource.Id.txtEmail);
            saveButton = view.FindViewById<Button>(Resource.Id.btnSave);
            materialSpinner = view.FindViewById<MaterialSpinner>(Resource.Id.spinner);
            saveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string fullname, email, phone, city, country;
            fullname = txtName.EditText.Text;
            city = txtCity.EditText.Text;
            country = txtCountry.EditText.Text;
            phone = txtPhone.EditText.Text;
            email = txtEmail.EditText.Text;
            

            if (fullname.Length < 5)
            {
                Toast.MakeText(Activity, "Please provide a valid name", ToastLength.Short).Show();
                return;
            }
            else if (!email.Contains("@"))
            {
                Toast.MakeText(Activity, "Please provide a valid email", ToastLength.Short).Show();
                return;
            }
            else if (phone.Length < 10)
            {
                Toast.MakeText(Activity, "Please provide a valid number", ToastLength.Short).Show();
                return;
            }
            else if (city.Length < 3)
            {
                Toast.MakeText(Activity, "Please provide a valid city", ToastLength.Short).Show();
                return;
            }
            else if (country.Length < 3)
            {
                Toast.MakeText(Activity, "Please provide a valid country", ToastLength.Short).Show();
                return;
            }
            Donor donor = new Donor();
            donor.Email = email;
            donor.Phone = phone;    
            donor.City = city;
            donor.Country = country;
            donor.FullName = fullname;
            donor.BloodGroup = selectedBloodGroup;

            OnDonorRegistered?.Invoke(this, new DonorDetailsEventArgs{  Donor = donor});
        }

        void SetupSpinner()
        {
            bloodGroups = new List<string>();
            bloodGroups.Add("O+");
            bloodGroups.Add("O-");
            bloodGroups.Add("A+");
            bloodGroups.Add("A-");
            bloodGroups.Add("AB+");
            bloodGroups.Add("AB-");
            bloodGroups.Add("B+");
            bloodGroups.Add("B-");

            spinnerAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, bloodGroups);
            spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            materialSpinner.Adapter = spinnerAdapter;
            materialSpinner.ItemSelected += MaterialSpinner_ItemSelected;
        }

        private void MaterialSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
                selectedBloodGroup = bloodGroups[e.Position];
                Console.WriteLine(selectedBloodGroup);
            }
        }
    }
}