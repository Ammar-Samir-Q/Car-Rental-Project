using CarRental_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class frmAddUpdateVehicle : Form
    {
        private enum enMode { AddNew=1,Update=2}
        private int _VehicleID;
        private clsVehicle _Vehicle;
        private enMode _Mode;

        public frmAddUpdateVehicle()
        {
            _Mode= enMode.AddNew;
            InitializeComponent();
        }

        public frmAddUpdateVehicle(int Vehicle)
        {
            _VehicleID = Vehicle;
            _Mode = enMode.Update;
            InitializeComponent();
        }

        //------------------------------Load---------------------------------

        private void _LoadComboBox()
        {
            cbFuel.Items.Add("Petrol");
            cbFuel.Items.Add("Diesel");
            cbFuel.Items.Add("Gas");
            cbFuel.Items.Add("Electric");
            cbFuel.Items.Add("Hybird");
            cbFuel.SelectedIndex = 0;

            cbCategory.Items.Add("Micro");
            cbCategory.Items.Add("Sedan");
            cbCategory.Items.Add("HatchBack");
            cbCategory.Items.Add("Universal");
            cbCategory.Items.Add("LiftBack");
            cbCategory.Items.Add("MuscleCar");
            cbCategory.Items.Add("SportCar");
            cbCategory.Items.Add("SUV");
            cbCategory.Items.Add("Crossover");
            cbCategory.Items.Add("Pickup");
            cbCategory.Items.Add("Van");
            cbCategory.SelectedIndex = 1;


        }
        private void _SetDefaults()
        {
            _LoadComboBox();

            _Vehicle=new clsVehicle();
           

            lblVehicleID.Text = "[???]";
            txtMake.Text = "";
            txtModel.Text = "";
            txtMilage.Text = "";
            txtPlateNumber.Text = "";
            txtRentalPrice.Text = "";
            txtYear.Text="";

            cbIsAvailable.Checked = true;

            btnRemoveImage.Visible=false;

            pbVehicleImage.Image = Properties.Resources.png_DefaultCar;


        }

        private void _LoadData()
        {
            _Vehicle = clsVehicle.GetVehicleByID(_VehicleID);
            if(_Vehicle ==null)
            {
                MessageBox.Show("Error While Loading Vehicle Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (Properties.Settings.Default.Language == "en")
            {
                lblTitle.Text = "Update-Vehicle";
                this.Text = lblTitle.Text;
            }
            else
            {
                lblTitle.Text = "تحديث مركبة";
                this.Text = lblTitle.Text;
            }
            lblVehicleID.Text=_Vehicle.VehicleID.ToString();
            txtMake.Text = _Vehicle.Make;
            txtModel.Text = _Vehicle.Model;
            txtMilage.Text = _Vehicle.CurrentMilage.ToString();
            txtPlateNumber.Text=_Vehicle.PlateNumber.ToString();
            txtYear.Text    = _Vehicle.Year.ToString();
            txtRentalPrice.Text=_Vehicle.RentalPricePerDay.ToString();

            cbFuel.SelectedValue = _Vehicle.FuelType;
            cbCategory.SelectedValue = _Vehicle.VehicleCategory;
            cbIsAvailable.Checked = _Vehicle.IsAvailableForRent;


            if (!string.IsNullOrEmpty(_Vehicle.ImagePath))
            {
                pbVehicleImage.ImageLocation = _Vehicle.ImagePath;
                btnRemoveImage.Visible = true;
            }
            else
                pbVehicleImage.Image = Properties.Resources.png_DefaultCar;
        }

        private void frmAddUpdateVehicle_Load(object sender, EventArgs e)
        {
            _SetDefaults();
            if(_Mode==enMode.Update)
                _LoadData();
        }

        //------------------------------Load---------------------------------

        //------------------------------Save---------------------------------


        private bool _HandelImage()
        {
            if (string.IsNullOrEmpty(pbVehicleImage.ImageLocation))
                return true;

            if(pbVehicleImage.ImageLocation==_Vehicle.ImagePath)
            {
                return true;
            }
            else
            {
                if(File.Exists(_Vehicle.ImagePath))
                {
                    try
                    {
                        File.Delete(_Vehicle.ImagePath);
                    }
                    catch 
                    {
                        return false;
                    }

                }

                string DestnitionFolder = AppDomain.CurrentDomain.BaseDirectory;
                if(!Directory.Exists(DestnitionFolder))
                {
                    Directory.CreateDirectory(DestnitionFolder);
                }

                Guid guid = Guid.NewGuid();
                FileInfo info = new FileInfo(pbVehicleImage.ImageLocation);
                string Ext=info.Extension;
                string DestnitionPath = DestnitionFolder + guid.ToString() + Ext;

                File.Copy(pbVehicleImage.ImageLocation,DestnitionPath,true);
                pbVehicleImage.ImageLocation = DestnitionPath;
                _Vehicle.ImagePath = DestnitionPath;
                return true;

            }





        }
        private void _Save()
        {
            _Vehicle.Make = txtMake.Text;
            _Vehicle.Model = txtModel.Text;
            _Vehicle.Year=int.Parse(txtYear.Text);
            _Vehicle.CurrentMilage=int.Parse(txtMilage.Text);
            _Vehicle.FuelType=(clsVehicle.enFuelType)(1+cbFuel.SelectedIndex);
            _Vehicle.VehicleCategory = (clsVehicle.enVehicleCategory)(1+cbCategory.SelectedIndex);
            _Vehicle.PlateNumber= txtPlateNumber.Text;
            _Vehicle.RentalPricePerDay=int.Parse(txtRentalPrice.Text);
            _Vehicle.IsAvailableForRent = cbIsAvailable.Checked;
            

            if(!_HandelImage())
            {
                MessageBox.Show("Error : Error While Handling The Image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_Vehicle.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblVehicleID.Text = _Vehicle.VehicleID.ToString();
                if (Properties.Settings.Default.Language == "en")
                {
                    lblTitle.Text = "Update-Vehicle";
                    this.Text = lblTitle.Text;
                }
                else
                {
                    lblTitle.Text = "تحديث مركبة";
                    this.Text = lblTitle.Text;
                }
                _Mode = enMode.Update;
                return;
            }
            else
            {
                MessageBox.Show("Error : Error While Saving The Data", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if(this.ValidateChildren())
                    _Save();
        }


        //------------------------------Save---------------------------------

        //------------------------------Misc---------------------------------

        private void _SetImage()
        {
            OpenFileDialog ofd=new OpenFileDialog();
            ofd.Title = "Choose Vehicle Image";
            
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                pbVehicleImage.ImageLocation = ofd.FileName;
                btnRemoveImage.Visible = true;
            }
            else
            {
                pbVehicleImage.Image = Properties.Resources.png_DefaultCar;
                btnRemoveImage.Visible = false;
            }
          
        }

        private void _RemoveImage()
        {
            if(!string.IsNullOrEmpty(_Vehicle.ImagePath))
            {

                if(File.Exists(_Vehicle.ImagePath))
                {
                    File.Delete(_Vehicle.ImagePath);
                   
                }
                pbVehicleImage.ImageLocation = "";
                _Vehicle.ImagePath = "";
            }
            pbVehicleImage.Image= Properties.Resources.png_DefaultCar;
            btnRemoveImage.Visible = false;


        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            _SetImage();
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _RemoveImage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        //------------------------------Misc---------------------------------

        //------------------------------Validation---------------------------------

        private void txtMake_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMake.Text))
            {
                e.Cancel = true;
                ep.SetError(txtMake, "Enter Make");
            }
            else
            {
                e.Cancel = false;
                ep.SetError(txtMake, "");

            }
        }

        private void txtModel_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtModel.Text))
            {
                e.Cancel = true;
                ep.SetError(txtModel, "Enter Model");
            }
            else
            {
                e.Cancel = false;
                ep.SetError(txtModel, "");

            }
        }

        private void txtYear_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtYear.Text))
            {
                e.Cancel = true;
                ep.SetError(txtYear, "Enter Year");
            }
            else
            {
                e.Cancel = false;
                ep.SetError(txtYear, "");

            }
        }

        private void txtMilage_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMilage.Text))
            {
                e.Cancel = true;
                ep.SetError(txtMilage, "Enter Milage");
            }
            else
            {
                e.Cancel = false;
                ep.SetError(txtMilage, "");

            }
        }

        private void txtPlateNumber_Validating(object sender, CancelEventArgs e)
        {
            if(clsVehicle.IsVehicleExists(txtPlateNumber.Text))
            {
                e.Cancel = true;
                ep.SetError(txtPlateNumber, "PlateNumber Already Exists");
            }


            if (string.IsNullOrEmpty(txtPlateNumber.Text))
            {
                e.Cancel = true;
                ep.SetError(txtPlateNumber, "Enter Plate Number");
            }
            else
            {
                e.Cancel = false;
                ep.SetError(txtPlateNumber, "");

            }
        }

        private void txtRentalPrice_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtRentalPrice.Text))
            {
                e.Cancel = true;
                ep.SetError(txtRentalPrice, "Enter Price");
            }
            else
            {
                e.Cancel = false;
                ep.SetError(txtRentalPrice, "");

            }
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled=!char.IsDigit(e.KeyChar)&&!char.IsControl(e.KeyChar);
        }

        private void txtMilage_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtRentalPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnSetImage_Click_1(object sender, EventArgs e)
        {
            _SetImage();
        }

        private void btnRemoveImage_Click_1(object sender, EventArgs e)
        {
            _RemoveImage();
        }

       

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }









        //------------------------------Validation---------------------------------


    }
}
