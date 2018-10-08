using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Radius_Log_Browser
{
    public partial class Form1 : Form
    {
        private long currentPosition;
        private Dictionary<string, string> type = new Dictionary<string, string>();
        private Dictionary<string, string> reason = new Dictionary<string, string>();
        private Dictionary<string, RadiusRequest> requests = new Dictionary<string, RadiusRequest>();
        private ListViewColumnSorter lvwColumnSorter;
        private string search;
        private int searchKey;
        private Dictionary<int, ListViewItem> oldItems = new Dictionary<int, ListViewItem>();
        private Dictionary<int, ListViewItem> newItems = new Dictionary<int, ListViewItem>();

        private string Lines;
        private string Directory;
        private string FileName;

        public Form1()
        {
            InitializeComponent();

            lvwColumnSorter = new ListViewColumnSorter();
            this.lvLogTable.ListViewItemSorter = lvwColumnSorter;

            // Add required columns
            lvLogTable.Columns.Add("Timestamp");
            lvLogTable.Columns.Add("Type");
            lvLogTable.Columns.Add("NAP Server");
            lvLogTable.Columns.Add("Access Point IP");
            lvLogTable.Columns.Add("Access Point Name");
            lvLogTable.Columns.Add("MAC-address requester");
            lvLogTable.Columns.Add("SAM-Account-Name");
            lvLogTable.Columns.Add("Responce Type");
            lvLogTable.Columns.Add("Reason Code");

            lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void procesLogFile()
        {
            XDocument doc = XDocument.Parse("<events>"+ Lines + "</events>");
            string classID;

            foreach (var events in doc.Descendants("Event"))
            {
                classID = events.Element("Class").Value;


                if (requests.ContainsKey(classID))
                {
                    requests[classID].setResponce(events);

                    ListViewItem item = new ListViewItem(new string[]
                    {
                        requests[classID].timestamp,
                        requests[classID].getRequestType(),
                        requests[classID].server,
                        requests[classID].accessPointIP,
                        requests[classID].accessPointName,
                        requests[classID].requesterMacAddress,
                        requests[classID].samAccountName,
                        requests[classID].getResponceType(),
                        requests[classID].getReason()
                    });

                    item.BackColor = requests[classID].getRowColor();

                    this.Invoke(new MethodInvoker(delegate { lvLogTable.Items.Add(item); }));
                    if (cbScroll.Checked)
                    {
                        this.Invoke(new MethodInvoker(delegate { lvLogTable.Items[lvLogTable.Items.Count - 1].EnsureVisible(); }));
                    }                    
                }
                else
                {
                    requests[classID] = new RadiusRequest(events);
                }
                    
            }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() {  Filter = "Excel Workbook|*xlsx", ValidateNames = true })
            {
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                    Worksheet ws = (Worksheet)app.ActiveSheet;
                    app.Visible = false;
                    ws.Cells[1, 1] = "Timestamp";
                    ws.Cells[1, 2] = "Type";
                    ws.Cells[1, 3] = "Computername";
                    ws.Cells[1, 4] = "Access Point IP";
                    ws.Cells[1, 5] = "Access Point Name";
                    ws.Cells[1, 6] = "MAC-Address Requester";
                    ws.Cells[1, 7] = "SAM-Account-Name";
                    ws.Cells[1, 8] = "Responce Type";
                    ws.Cells[1, 9] = "Reason Code";

                    int i = 2;

                    foreach(ListViewItem item in lvLogTable.Items)
                    {
                        ws.Cells[i, 1] = item.SubItems[0].Text;
                        ws.Cells[i, 2] = item.SubItems[1].Text;
                        ws.Cells[i, 3] = item.SubItems[2].Text;
                        ws.Cells[i, 4] = item.SubItems[3].Text;
                        ws.Cells[i, 5] = item.SubItems[4].Text;
                        ws.Cells[i, 6] = item.SubItems[5].Text;
                        ws.Cells[i, 7] = item.SubItems[6].Text;
                        ws.Cells[i, 8] = item.SubItems[7].Text;
                        ws.Cells[i, 9] = item.SubItems[8].Text;
                        i++;
                    }

                    wb.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false,XlSaveAsAccessMode.xlNoChange,XlSaveConflictResolution.xlLocalSessionChanges,Type.Missing,Type.Missing);
                    app.Quit();

                    MessageBox.Show("Log has been succesfully exported to Excel","Succesful",MessageBoxButtons.OK,MessageBoxIcon.Information);

                }
            }
        }

        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                lvLogTable.Items.Clear();
                currentPosition = 0;

                Directory = Path.GetDirectoryName(ofdFile.FileName);
                FileName = Path.GetFileName(ofdFile.FileName);

                ReadLines(ofdFile.FileName);
                procesLogFile();

                lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                RunWatcher();
            }
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help helpdialog = new help();
            helpdialog.FormBorderStyle = FormBorderStyle.FixedSingle;
            helpdialog.ShowDialog();
        }

        private void lvLogTable_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) != 0)
            {
                if (e.KeyCode == Keys.C)
                {
                    CopySelectedValuesToClipboard();
                }
                else if (e.KeyCode == Keys.A)
                {
                    foreach (ListViewItem item in lvLogTable.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void lvLogTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvLogTable.Columns[lvwColumnSorter.SortColumn].ImageIndex = -1;
            lvLogTable.Columns[lvwColumnSorter.SortColumn].TextAlign = HorizontalAlignment.Left;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                    lvLogTable.Columns[lvwColumnSorter.SortColumn].ImageIndex = 1;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                    lvLogTable.Columns[lvwColumnSorter.SortColumn].ImageIndex = 0;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
                lvLogTable.Columns[lvwColumnSorter.SortColumn].ImageIndex = 0;
            }

            // Perform the sort with these new sort options.
            this.lvLogTable.Sort();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in lvLogTable.SelectedItems)
            {
                builder.AppendLine(item.SubItems[0].Text +"\t"+ item.SubItems[1].Text + "\t" + item.SubItems[2].Text + "\t" + item.SubItems[3].Text + "\t" + item.SubItems[4].Text + "\t" + item.SubItems[5].Text + "\t" + item.SubItems[6].Text + "\t" + item.SubItems[7].Text + "\t" + item.SubItems[8].Text);
            }

            if(builder.Length != 0)
            {
                Clipboard.SetText(builder.ToString());
            }
            
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedValuesToClipboard();
        }

        private void lvLogTable_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(lvLogTable.SelectedItems.Count == 0)
            {
                cmCopy.Enabled = false;
            }
            else
            {
                cmCopy.Enabled = true;
            }
        }

        private void lvLogTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo item = lvLogTable.HitTest(e.Location);

                if (item != null)
                {
                    if (item.SubItem == null)
                    {
                        cmSearch.Visible = false;
                        cmSearchSep.Visible = false;
                    }
                    else
                    {
                        cmSearch.Text = "Search on '" + item.SubItem.Text + "'";
                        cmSearch.Visible = true;
                        cmSearchSep.Visible = true;
                        search = item.SubItem.Text;

                        int key = 0;
                        foreach(ListViewItem.ListViewSubItem cols in item.Item.SubItems)
                        {
                            if (cols.Text == search)
                            {
                                searchKey = key;
                                break;
                            }
                            key++;
                        }
                    }

                }

                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void lvLogTable_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                var item = lvLogTable.HitTest(e.Location).Item;
                if (item != null)
                    item.Selected = true;
            }
            base.OnMouseMove(e);
        }

        private void cmSearch_Click(object sender, EventArgs e)
        {
            

            if(oldItems.Count == 0)
            {
                foreach (ListViewItem item in lvLogTable.Items)
                {
                    if(item != null)
                    {
                        oldItems.Add(item.Index, item);
                    }
                }
                cmRemoveSearch.Visible = true;
            }

            foreach (ListViewItem item in lvLogTable.Items)
            {
                if (item.SubItems[searchKey].Text == search)
                {
                    newItems.Add(item.Index, item);
                }
                
            }

            lvLogTable.Items.Clear();

            foreach (ListViewItem item in newItems.Values)
            {
                lvLogTable.Items.Add(item);
            }

            newItems.Clear();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvLogTable.Items.Clear();
            cmCopy.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
        }

        private void cmRemoveSearch_Click(object sender, EventArgs e)
        {
            cmRemoveSearch.Visible = false;
            lvLogTable.Items.Clear();

            foreach (ListViewItem item in oldItems.Values)
            {
                lvLogTable.Items.Add(item);
            }

            oldItems.Clear();
        }

        public void RunWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Directory;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = FileName;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;

        }


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            ReadLines(ofdFile.FileName);
            procesLogFile();
        }

        public void ReadLines(string path)
        {
        using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Seek(currentPosition, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    Lines = reader.ReadToEnd();

                    currentPosition = stream.Position;
                }
            }
        }

        private void cbScroll_Click(object sender, EventArgs e)
        {
            cbScroll.Checked = !cbScroll.Checked;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
