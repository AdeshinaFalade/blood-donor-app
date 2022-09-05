
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;

namespace blood_donor_app
{
    internal class AdapterDonors : RecyclerView.Adapter
    {
        public event EventHandler<AdapterDonorsClickEventArgs> ItemClick;
        public event EventHandler<AdapterDonorsClickEventArgs> ItemLongClick;
        public event EventHandler<AdapterDonorsClickEventArgs> CallClick;
        public event EventHandler<AdapterDonorsClickEventArgs> MailClick;
        public event EventHandler<AdapterDonorsClickEventArgs> DeleteClick;
        List<Donor> DonorsList;

        public AdapterDonors(List<Donor> data)
        {
            DonorsList = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(id, parent, false);

            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.donor_row, parent, false);   

            var vh = new AdapterDonorsViewHolder(itemView, OnClick, OnLongClick, OnCallClick, OnMailClick, OnDeleteClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var donor = DonorsList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as AdapterDonorsViewHolder;
            holder.donorNameTextView.Text = donor.FullName;
            holder.donorLocationTextView.Text = donor.City + ", " + donor.Country;
            //holder.TextView.Text = items[position];

            //assign appropriate images
            if (donor.BloodGroup == "O+")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.o_positive);
            }
            else if (donor.BloodGroup == "O-")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.o_negative);
            }
            else if (donor.BloodGroup == "A+")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.a_positive);
            }
            else if (donor.BloodGroup == "A-")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.a_negative);
            }
            else if (donor.BloodGroup == "B+")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.b_positive);
            }
            else if (donor.BloodGroup == "B-")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.b_negative);
            }
            else if (donor.BloodGroup == "AB+")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.ab_positive);
            }
            else if (donor.BloodGroup == "AB-")
            {
                holder.bloodGroupImageView.SetImageResource(Resource.Drawable.ab_negative);
            }
        }

        public override int ItemCount => DonorsList.Count;

        void OnClick(AdapterDonorsClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(AdapterDonorsClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        void OnCallClick(AdapterDonorsClickEventArgs args) => CallClick?.Invoke(this, args);
        void OnMailClick(AdapterDonorsClickEventArgs args) => MailClick?.Invoke(this, args);
        void OnDeleteClick(AdapterDonorsClickEventArgs args) => DeleteClick?.Invoke(this, args);


    }

    public class AdapterDonorsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView donorNameTextView;
        public TextView donorLocationTextView;   
        public ImageView bloodGroupImageView;
        public RelativeLayout callLayout;
        public RelativeLayout mailLayout;
        public RelativeLayout deleteLayout;


        public AdapterDonorsViewHolder(View itemView, Action<AdapterDonorsClickEventArgs> clickListener,
                            Action<AdapterDonorsClickEventArgs> longClickListener, Action<AdapterDonorsClickEventArgs> callClickListener,
                            Action<AdapterDonorsClickEventArgs> mailClickListener,
                            Action<AdapterDonorsClickEventArgs> deleteClickListener) : base(itemView)
        {
            //TextView = v;
            donorNameTextView = itemView.FindViewById<TextView>(Resource.Id.txtDonorName);
            donorLocationTextView = (TextView)itemView.FindViewById(Resource.Id.txtLocation);
            bloodGroupImageView = (ImageView)itemView.FindViewById(Resource.Id.bloodGroupImageView);
            callLayout = (RelativeLayout)itemView.FindViewById(Resource.Id.callLayout);
            mailLayout = (RelativeLayout)itemView.FindViewById(Resource.Id.emailLayout);
            deleteLayout = (RelativeLayout)itemView.FindViewById(Resource.Id.deleteLayout);
            itemView.Click += (sender, e) => clickListener(new AdapterDonorsClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new AdapterDonorsClickEventArgs { View = itemView, Position = AdapterPosition });
            callLayout.Click += (sender,e) => callClickListener(new AdapterDonorsClickEventArgs { View = itemView, Position = AdapterPosition });
            mailLayout.Click += (sender,e) => mailClickListener(new AdapterDonorsClickEventArgs { View = itemView, Position = AdapterPosition });
            deleteLayout.Click += (sender, e) => deleteClickListener(new AdapterDonorsClickEventArgs { View = itemView, Position = AdapterPosition });  
        }
    }

    public class AdapterDonorsClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}