using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitungDiskon
{
    public partial class Form1 : Form
    {
        private DataTable dtsource;
        public Form1()
        {
            InitializeComponent();
            loadGridAwal();
            bindCmbUOM();
            loadAwal();
        }

        private void loadAwal()
        {
            txtDiscount.Text = "";
            txtDiscountAmt.Text = "";
            txtItemDesc.Text = "";
            txtQty.Text = "";
            txtQtyKecil.Text = "";
            txtTotal.Text = "";
            txtUnitPrice.Text = "";
            cmbUOM.SelectedIndex = 0;

            txtRate.Text = cmbUOM.SelectedValue.ToString();

        }

        private void bindCmbUOM()
        {
            cmbUOM.DataSource = dgvUOM.DataSource;
            cmbUOM.DisplayMember = "Satuan";
            cmbUOM.ValueMember = "Rate";
            cmbUOM.SelectedIndex = 0;

            txtRate.Text = cmbUOM.SelectedValue.ToString();
        }

        private void loadGridAwal()
        {
            dtsource = new DataTable();
            dtsource.Columns.Add("Satuan", typeof(string));
            dtsource.Columns.Add("Rate", typeof(int));
            DataRow row = dtsource.NewRow();
            row[0] = "PCS";
            row[1] = 1;
            dtsource.Rows.Add(row);
            row = dtsource.NewRow();
            row[0] = "LUSIN";
            row[1] = 12;
            dtsource.Rows.Add(row);
            row = dtsource.NewRow();
            row[0] = "BOX";
            row[1] = 24;
            dtsource.Rows.Add(row);
            dtsource.AcceptChanges();

            dgvUOM.DataSource = dtsource;
            dgvUOM.Columns[0].ReadOnly = true;
        }

        private void btnSaveUOM_Click(object sender, EventArgs e)
        {
            dtsource = new DataTable();
            dtsource.Columns.Add("Satuan", typeof(string));
            dtsource.Columns.Add("Rate", typeof(int));
            foreach (DataGridViewRow rowview in dgvUOM.Rows)
            {
                if (rowview.Cells[0].ToString() != "" && rowview.Cells[1].ToString() != "")
                {
                    DataRow row = dtsource.NewRow();
                    row[0] = rowview.Cells[0].Value;
                    row[1] = rowview.Cells[1].Value;
                    dtsource.Rows.Add(row);
                    row = dtsource.NewRow();
                }
            }
            dgvUOM.DataSource = null;
            dgvUOM.DataSource = dtsource;
            dgvUOM.Columns[0].ReadOnly = true;
            bindCmbUOM();
            loadAwal();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dgvUOM_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgvUOM.CurrentCell.ColumnIndex == 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmbUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRate.Text = cmbUOM.SelectedValue.ToString();

            if (txtQty.Text != "")
            {
                txtQtyKecil.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtRate.Text)).ToString();
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != "")
            {
                txtQtyKecil.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtRate.Text)).ToString();
                if (txtUnitPrice.Text != "")
                {
                    if (txtDiscount.Text != "")
                    {
                        if (!txtDiscount.Text.Contains("%") && !txtDiscount.Text.Contains("+"))
                        {
                            txtDiscountAmt.Text = txtDiscount.Text;
                            txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscount.Text)).ToString();
                        }
                        else
                        {
                            try
                            {
                                if (txtDiscount.Text.Contains("+"))
                                {
                                    string dis = txtDiscount.Text.Replace("%", "");
                                    string[] arrdis = dis.Split('+');
                                    int dsk = 0;
                                    for (int i = 0; i < arrdis.Length; i++)
                                    {
                                        if (arrdis[i] != "")
                                        {
                                            dsk += Convert.ToInt32(arrdis[i]);
                                        }
                                    }

                                    txtDiscountAmt.Text = ((Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) * dsk) / 100).ToString();
                                    txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscountAmt.Text)).ToString();
                                }
                                else
                                {
                                    int dsk = Convert.ToInt32(txtDiscount.Text.Replace("%", ""));

                                    txtDiscountAmt.Text = ((Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) * dsk) / 100).ToString();
                                    txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscountAmt.Text)).ToString();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Salah format diskon");
                            }
                        }
                    }
                    else
                    {
                        txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text)).ToString();
                    }
                }
            }
            else
            {
                txtQtyKecil.Text = "";
                txtTotal.Text = "";
            }

        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtUnitPrice.Text != "")
            {
                if (txtDiscount.Text != "")
                {
                    if (txtQty.Text != "")
                    {
                        if (!txtDiscount.Text.Contains("%") && !txtDiscount.Text.Contains("+"))
                        {
                            txtDiscountAmt.Text = txtDiscount.Text;
                            txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscount.Text)).ToString();
                        }
                        else
                        {
                            try
                            {
                                if (txtDiscount.Text.Contains("+"))
                                {
                                    string dis = txtDiscount.Text.Replace("%", "");
                                    string[] arrdis = dis.Split('+');
                                    int dsk = 0;
                                    for (int i = 0; i < arrdis.Length; i++)
                                    {
                                        if (arrdis[i] != "")
                                        {
                                            dsk += Convert.ToInt32(arrdis[i]);
                                        }
                                    }

                                    txtDiscountAmt.Text = ((Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) * dsk) / 100).ToString();
                                    txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscountAmt.Text)).ToString();
                                }
                                else
                                {
                                    int dsk = Convert.ToInt32(txtDiscount.Text.Replace("%", ""));

                                    txtDiscountAmt.Text = ((Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) * dsk) / 100).ToString();
                                    txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscountAmt.Text)).ToString();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Salah format diskon");
                            }
                        }
                    }
                }
                else
                {
                    if (txtQty.Text != "")
                    {
                        txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text)).ToString();
                    }
                }
            }
            else
            {
                txtTotal.Text = "";
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscount.Text != "" && txtQty.Text != "" && txtUnitPrice.Text != "")
            {
                if (!txtDiscount.Text.Contains("%") && !txtDiscount.Text.Contains("+"))
                {
                    txtDiscountAmt.Text = txtDiscount.Text;
                    try { txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscount.Text)).ToString(); } catch { MessageBox.Show("Format Salah"); }
                }
                else
                {
                    try
                    {
                        if (txtDiscount.Text.Contains("+"))
                        {
                            string dis = txtDiscount.Text.Replace("%","");
                            string[] arrdis = dis.Split('+');
                            int dsk = 0;
                            for (int i = 0; i < arrdis.Length; i++)
                            {
                                if (arrdis[i]!="")
                                {
                                    dsk += Convert.ToInt32(arrdis[i]);
                                }
                            }
                            
                            txtDiscountAmt.Text = ((Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) * dsk) / 100).ToString();
                            txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscountAmt.Text)).ToString();
                        }
                        else
                        {
                            int dsk = Convert.ToInt32(txtDiscount.Text.Replace("%", ""));

                            txtDiscountAmt.Text = ((Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) * dsk) / 100).ToString();
                            txtTotal.Text = (Convert.ToInt32(txtQty.Text) * Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(txtDiscountAmt.Text)).ToString();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Salah format diskon");
                    }
                }
            }
        }
    }
}
