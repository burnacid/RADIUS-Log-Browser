using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Radius_Log_Browser
{
    public partial class Form1 : Form
    {
        private String file;
        private Dictionary<string, string> type = new Dictionary<string, string>();
        private Dictionary<string, string> reason = new Dictionary<string, string>();
        private Dictionary<string, RadiusRequest> requests = new Dictionary<string, RadiusRequest>();
        private ListViewColumnSorter lvwColumnSorter;

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

            type["1"] = "Access-Request";
            type["2"] = "Access-Accept";
            type["3"] = "Access-Reject";
            type["4"] = "Accounting-Request";
            type["5"] = "Accounting-Response";
            type["11"] = "Access-Challenge";
            type["12"] = "Status-Server (experimental)";
            type["13"] = "Status-Client (experimental)";
            type["255"] = "Reserved";

            reason["0"] = "The connection request was successfully authenticated and authorized by Network Policy Server.";
            reason["1"] = "The connection request failed due to a Network Policy Server error.";
            reason["2"] = "There are insufficient access rights to process the request.";
            reason["3"] = "The Remote Authentication Dial-In User Service (RADIUS) Access-Request message that NPS received from the network access server was malformed.";
            reason["4"] = "The NPS server was unable to access the Active Directory Domain Services (AD DS) global catalog. Because of this, authentication and authorization for the connection request cannot be performed, and access is denied.";
            reason["5"] = "The Network Policy Server was unable to connect to a domain controller in the domain where the user account is located. Because of this, authentication and authorization for the connection request cannot be performed, and access is denied.";
            reason["6"] = "The NPS server is unavailable. This issue can occur if the NPS server is running low on or is out of random access memory (RAM). It can also occur if the NPS server fails to receive the name of a domain controller, if there is a problem with the Security Accounts Manager (SAM) database on the local computer, or in circumstances where there is a Windows NT directory service (NTDS) failure.";
            reason["7"] = "The domain that is specified in the User-Name attribute of the RADIUS message does not exist.";
            reason["8"] = "The user account that is specified in the User-Name attribute of the RADIUS message does not exist.";
            reason["9"] = "An Internet Authentication Service (IAS) extension dynamic link library (DLL) that is installed on the NPS server discarded the connection request.";
            reason["10"] = "An IAS extension dynamic link library (DLL) that is installed on the NPS server has failed and cannot perform its function.";
            reason["16"] = "Authentication failed due to a user credentials mismatch. Either the user name provided does not match an existing user account or the password was incorrect.";
            reason["17"] = "The user's attempt to change their password has failed.";
            reason["18"] = "The authentication method used by the client computer is not supported by Network Policy Server for this connection. With CHAP, reversibly encrypted password storage is required. You can enable reversibly encrypted password storage per user account or for all accounts in a domain using Group Policy. To enable reversibly encrypted password storage for a user account, obtain the properties of a user account in AD DS, click the Account tab, and then select the Store password using reversible encryption check box. To allow reversibly encrypted password storage for all user accounts in the domain, add the Group Policy Management Editor snap-in to the Microsoft Management Console (MMC) and enable the default domain policy setting Store password using reversible encryption at the following path: Computer Configuration | Policies | Windows Settings | Security Settings | Account Policies | Password Policies.";
            reason["20"] = "The client attempted to use LAN Manager authentication, which is not supported by Network Policy Server. To enable the use of LAN Manager authentication, see NPS: LAN Manager Authentication.";
            reason["21"] = "An IAS extension dynamic link library (DLL) that is installed on the NPS server rejected the connection request.";
            reason["22"] = "Network Policy Server was unable to negotiate the use of an Extensible Authentication Protocol (EAP) type with the client computer.";
            reason["23"] = "An error occurred during the Network Policy Server use of the Extensible Authentication Protocol (EAP). Check EAP log files for EAP errors. By default, these log files are located at %windir%\\System32\\Logfiles.";
            reason["32"] = "NPS is joined to a workgroup and performs the authentication and authorization of connection requests using the local Security Accounts Manager database, however the Access-Request message contains a domain user name. NPS does not have access to a domain user accounts database. The connection request was denied.";
            reason["33"] = "The user that is attempting to connect to the network must change their password.";
            reason["34"] = "The user account that is specified in the RADIUS Access-Request message is disabled.";
            reason["35"] = "The user account that is specified in the RADIUS Access-Request message is expired.";
            reason["36"] = "The user's authentication attempts have exceeded the maximum allowed number of failed attempts specified by the Account lockout thresholdsetting in Account Lockout Policyin Group Policy. To unlock the account, obtain the user account properties in the Active Directory Users and Computers Microsoft Management Console(MMC) snap -in, click theAccount tab, and then clickUnlock account.";
            reason["37"] = "According to AD DS user account logon hours, the user is not permitted to access the network on this day and time. To change the account logon hours, obtain the user account properties in the Active Directory Users and Computers snap-in, click the Accounttab, and then click Logon Hours. In the Logon Hours dialog box, configure the days and times when the user is permitted to access the network.";
            reason["38"] = "Authentication failed due to a user account restriction or requirement that was not followed. For example, the user account settings might require the use of a password but the user attempted to log on with a blank password. To resolve this issue, check the user account properties for restrictions, and then either remove or modify the restrictions or inform the user of the requirements for network access.";
            reason["48"] = "The connection request did not match a configured network policy, so the connection request was denied by Network Policy Server.";
            reason["49"] = "The connection request did not match a configured connection request policy, so the connection request was denied by Network Policy Server.";
            reason["64"] = "Remote Access Account Lockout is enabled, and the user's authentication attempts have exceeded the designated lockout count because the credentials they supplied (user name and password) are not valid.When the lockout count for a user account is reset to 0 due to either a successful authentication or an automatic reset, the registry subkey for the user account is deleted.To manually reset a user account that has been locked out before the failed attempts counter is automatically reset, delete the following registry subkey that corresponds to the user's account name:HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\RemoteAccess\\Parameters\\AccountLockout\\domain name:user name";
            reason["65"] = "The Network Access Permission setting in the dial-in properties of the user account is set to Deny access to the user. To change the Network Access Permission setting to either Allow access or Control access through NPS Network Policy, obtain the properties of the user account in the Active Directory Users and Computers Microsoft Management Console (MMC) snap-in, click the Dial-in tab, and then change Network Access Permission.";
            reason["66"] = "Authentication failed. Either the client computer attempted to use an authentication method that is not enabled on the matching network policy or the client computer attempted to authenticate as Guest, but guest authentication is not enabled. To resolve this issue, ensure that all client computers are configured to use one or more authentication methods that are allowed by matching network policies.";
            reason["67"] = "NPS denied the connection request because the value of the Calling-Station-ID attribute in the Access-Request message did not match the value of Verify Caller ID in user account dial-in properties in the Active Directory Users and Computers snap-in.";
            reason["68"] = "The user or computer does not have permission to access the network on this day at this time. To change the day and time when the user is permitted to connect to the network, change the Day and Time Restrictions in the constraints of the matching network policy. For more information, see Constraints Properties.";
            reason["69"] = "The telephone number of the network access server does not match the value of the Calling-Station-ID attribute that is configured in the constraints of the matching network policy. NPS denied the Access-Request message.";
            reason["70"] = "The network access method used by the access client to connect to the network does not match the value of the NAS-Port-Type attribute that is configured in the constraints of the matching network policy. NPS denied the Access-Request message.";
            reason["72"] = "The user password has expired or is about to expire and the user must change their password, however Authentication Methodsin network policy constraints are not configured to allow the user to change their password. To allow the user to change their password, open the properties of the matching network policy, click the Constraints tab, clickAuthentication Methods, and then in the details pane select the appropriate authentication method and User can change password after it has expired check box.";
            reason["73"] = "The purposes that are configured in the Application Policies extensions, also called Enhanced Key Usage (EKU) extensions, section of the user or computer certificate are not valid or are missing. The user or computer certificate must be configured with the Client Authentication purpose in Application Policies extensions. The object identifier for Client Authentication is 1.3.6.1.5.5.7.3.2. To correct this problem, you must reconfigure the certificate template with the Client Authentication purpose in Application Policies extensions, revoke the old certificate, and enroll a new certificate that is configured correctly. For more information, see Foundation Network Companion Guide: Deploying Computer and User Certificates at http://go.microsoft.com/fwlink/?LinkId=113884.";
            reason["80"] = "NPS attempted to write accounting data to the data store (a log file on the local computer or a SQL Server database), but failed to do so for unknown reasons.";
            reason["96"] = "Authentication failed due to an Extensible Authentication Protocol (EAP) session timeout; the EAP session with the access client was incomplete.";
            reason["97"] = "The authentication request was not processed because it contained a Remote Authentication Dial-In User Service (RADIUS) message that was not appropriate for the secure authentication transaction.";
            reason["112"] = "The local NPS proxy server forwarded a connection request to a remote RADIUS server, and the remote server rejected the connection request. Check the event log on the remote RADIUS server to determine the reason that the connection request was rejected.";
            reason["113"] = "The local NPS proxy attempted to forward a connection request to a member of a remote RADIUS server group that does not exist. To resolve this issue, configure a valid remote RADIUS server group.";
            reason["115"] = "The local NPS proxy did not forward a RADIUS message because it is not an accounting request or a connection request.";
            reason["116"] = "The local NPS proxy server cannot forward the connection request to the remote RADIUS server because either the proxy cannot open a Windows socket over which to send the connection request, or the proxy server attempted to send the connection request but received Windows sockets errors that prevented successful completion of the send operation.";
            reason["117"] = "The remote RADIUS server did not respond to the local NPS proxy within an acceptable time period. Verify that the remote RADIUS server is available and functioning properly.";
            reason["118"] = "The local NPS proxy server received a RADIUS message that is malformed from a remote RADIUS server, and the message is unreadable. This issue can also be caused if a connection request contains more than the expected number of User-Name attributes, or if the User-Name attribute value is not valid, such as if the value has zero length or if it contains characters that are not valid.";
            reason["256"] = "The certificate provided by the user or computer as proof of their identity is a revoked certificate. Because of this, the user or computer was not authenticated, and NPS rejected the connection request.";
            reason["257"] = "Due to a missing dynamic link library (DLL) or exported function, NPS cannot access the certificate revocation list to verify whether the user or client computer certificate is valid or is revoked.";
            reason["258"] = "NPS cannot access the certificate revocation list to verify whether the user or client computer certificate is valid or is revoked. Because of this, authentication failed.";
            reason["259"] = "The certification authority that manages the certificate revocation list is not available. NPS cannot verify whether the certificate is valid or is revoked. Because of this, authentication failed.";
            reason["260"] = "The Extensible Authentication Protocol (EAP) message has been altered so that the Message Digest 5 (MD5) hash of the entire Remote Authentication Dial-In User Service (RADIUS) message does not match, or the message has been altered at the Schannel level.";
            reason["261"] = "NPS cannot contact Active Directory Domain Services (AD DS) or the local user accounts database to perform authentication and authorization. The connection request is denied for this reason.";
            reason["262"] = "NPS discarded the RADIUS message because it is incomplete and the signature was not verified.";
            reason["263"] = "NPS did not receive complete credentials from the user or computer. The connection request is denied for this reason.";
            reason["264"] = "The Security Support Provider Interface (SSPI) called by EAP reports that the system clocks on the NPS server and the access client are not synchronized.";
            reason["265"] = "The certificate that the user or client computer provided to NPS as proof of identity chains to an enterprise root certification authority that is not trusted by the NPS server.";
            reason["266"] = "NPS received a message that was either unexpected or incorrectly formatted. NPS discarded the message for this reason.";
            reason["267"] = "The certificate provided by the connecting user or computer is not valid because it is not configured with the Client Authentication purpose in Application Policies or Enhanced Key Usage (EKU) extensions. NPS rejected the connection request for this reason.";
            reason["268"] = "The certificate provided by the connecting user or computer is expired. NPS rejected the connection request for this reason.";
            reason["269"] = "The Security Support Provider Interface (SSPI) called by EAP reports that the NPS server and the access client cannot communicate because they do not possess a common algorithm.";
            reason["270"] = "Based on the matching NPS network policy, the user is required to log on with a smart card, but they have attempted to log on by using other credentials. NPS rejected the connection request for this reason.";
            reason["271"] = "The connection request was not processed because the NPS server was in the process of shutting down or restarting when it received the request.";
            reason["272"] = "The certificate that the user or client computer provided to NPS as proof of identity maps to multiple user or computer accounts rather than one account. NPS rejected the connection request for this reason.";
            reason["273"] = "Authentication failed. NPS called Windows Trust Verification Services, and the trust provider is not recognized on this computer. Atrust provider is a software module that implements the algorithm for application-specific policies regarding trust.";
            reason["274"] = "Authentication failed. NPS called Windows Trust Verification Services, and the trust provider does not support the specified action. Each trust provider provides its own unique set of action identifiers. For information about the action identifiers supported by a trust provider, see the documentation for that trust provider.";
            reason["275"] = "Authentication failed. NPS called Windows Trust Verification Services, and the trust provider does not support the specified form. Atrust provider is a software module that implements the algorithm for application-specific policies regarding trust. Trust providers support subject forms that describe where the trust information is located and what trust actions to take regarding the subject.";
            reason["276"] = "Authentication failed. NPS called Windows Trust Verification Services, but the binary file that calls EAP cannot be verified and is not trusted.";
            reason["277"] = "Authentication failed. NPS called Windows Trust Verification Services, but the binary file that calls EAP is not signed, or the signer certificate cannot be found.";
            reason["278"] = "Authentication failed. The certificate that was provided by the connecting user or computer is expired.";
            reason["279"] = "Authentication failed. The certificate is not valid because the validity periods of certificates in the chain do not match. For example, the following End Certificate and Issuer Certificate validity periods do not match: End Certificate validity period: 2007-2010; Issuer Certificate validity period: 2006-2008.";
            reason["280"] = "Authentication failed. The certificate is not valid and was not issued by a valid certification authority (CA).";
            reason["281"] = "Authentication failed. The path length constraint in the certification chain has been exceeded. This constraint restricts the maximum number of CA certificates that can follow this certificate in the certificate chain.";
            reason["282"] = "Authentication failed. The certificate contains a critical extension that is unrecognized by NPS.";
            reason["283"] = "Authentication failed. The certificate does not contain the Client Authentication purpose in Application Policies extensions, and cannot be used for authentication.";
            reason["284"] = "Authentication failed. The certificate is not valid because the certificate issuer and the parent of the certificate in the certificate chain are required to match but do not match.";
            reason["285"] = "Authentication failed. NPS cannot locate the certificate, or the certificate is incorrectly formed and is missing important information.";
            reason["286"] = "Authentication failed. The certificate provided by the connecting user or computer is issued by a certification authority (CA) that is not trusted by the NPS server.";
            reason["287"] = "Authentication failed. The certificate provided by the connecting user or computer does not chain to an enterprise root CA that NPS trusts.";
            reason["288"] = "Authentication failed due to an unspecified trust failure.";
            reason["289"] = "Authentication failed. The certificate provided by the connecting user or computer is revoked and is not valid.";
            reason["290"] = "Authentication failed. A test or trial certificate is in use, however the test root CA is not trusted, according to local or domain policy settings.";
            reason["291"] = "Authentication failed because NPS cannot locate and access the certificate revocation list to verify whether the certificate has or has not been revoked. This issue can occur if the revocation server is not available or if the certificate revocation list cannot be located in the revocation server database.";
            reason["292"] = "Authentication failed. The value of the User-Name attribute in the connection request does not match the value of the common name (CN) property in the certificate.";
            reason["293"] = "Authentication failed. The certificate provided by the connecting user or computer is not valid because it is not configured with the Client Authentication purpose in Application Policies or Enhanced Key Usage (EKU) extensions. NPS rejected the connection request for this reason.";
            reason["294"] = "Authentication failed because the certificate was explicitly marked as untrusted by the Administrator. Certificates are designated as untrusted when they are imported into the Untrusted Certificates folder in the certificate store for the Current User or Local Computer in the Certificates Microsoft Management Console (MMC) snap-in.";
            reason["295"] = "Authentication failed. The certificate provided by the connecting user or computer is issued by a CA that is not trusted by the NPS server.";
            reason["296"] = "Authentication failed. The certificate provided by the connecting user or computer is not valid because it is not configured with the Client Authentication purpose in Application Policies or Enhanced Key Usage (EKU) extensions. NPS rejected the connection request for this reason.";
            reason["297"] = "Authentication failed. The certificate provided by the connecting user or computer is not valid because it does not have a valid name.";
            reason["298"] = "Authentication failed. Either the certificate does not contain a valid user principal name (UPN) or the value of the User-Name attribute in the connection request does not match the certificate.";
            reason["299"] = "Authentication failed. The sequence of information provided by internal components or protocols during message verification is incorrect.";
            reason["300"] = "Authentication failed. The certificate is malformed and Extensible Authentication Protocl (EAP) cannot locate credential information in the certificate.";
            reason["301"] = "NPS terminated the authentication process. NPS received a cryptobinding type length value (TLV) from the access client that is not valid. This issue occurs when an attempt to breach your network security has occurred and a man-in-the-middle (MITM) attack is in progress. During MITM attacks on your network, attackers use unauthorized computers to intercept traffic between your legitimate hosts while posing as one of the legitimate hosts. The attacker's computer attempts to gain data from your other network resources.This enables the attacker to use the unauthorized computer to intercept, decrypt, and access all network traffic that would otherwise go to one of your legitimate network resources.";
            reason["302"] = "NPS terminated the authentication process. NPS did not receive a required cryptobinding type length value (TLV) from the access client during the authentication process."; ;


        }

        private void procesLogFile()
        {
            lvLogTable.Items.Clear();

            XDocument doc = XDocument.Parse("<events>"+file+"</events>");
            string classID;

            foreach (var events in doc.Descendants("Event"))
            {
                classID = events.Element("Class").Value;


                if (requests.ContainsKey(classID))
                {
                    requests[classID].setResponce(events);
                }
                else
                {
                    requests[classID] = new RadiusRequest(events);
                }
                
                /*
                if (events.Elements("NAS-Identifier").Any())
                {
                    NASIdentifier = events.Element("NAS-Identifier").Value;
                }
                else
                {
                    NASIdentifier = "";
                }

                if (events.Elements("Calling-Station-Id").Any())
                {
                    CallingStationId = events.Element("Calling-Station-Id").Value;
                }
                else
                {
                    CallingStationId = "";
                }

                if (events.Elements("Computer-Name").Any())
                {
                */
                    
            }

            foreach(RadiusRequest request in requests.Values)
            {
                ListViewItem item = new ListViewItem(new string[]
                    {
                request.timestamp,
                request.getRequestType(),
                request.server,
                request.accessPointIP,
                request.accessPointName,
                request.requesterMacAddress,
                request.samAccountName,
                request.getResponceType(),
                request.getReason()
                    });


                item.BackColor = request.getRowColor();


                lvLogTable.Items.Add(item);
            }

            lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
            loadFile();
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help helpdialog = new help();
            helpdialog.FormBorderStyle = FormBorderStyle.FixedSingle;
            helpdialog.ShowDialog();
        }

        private void loadFromAD001ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFile("\\\\file001\\data$\\Support\\ICT\\Log Files\\RADIUS\\AD001");
        }

        private void loadFromAD002ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFile("\\\\file001\\data$\\Support\\ICT\\Log Files\\RADIUS\\AD002");
        }

        private void loadFile(string defaultPath = "")
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Log files|*.log";
            openFileDialog1.Title = "Select a Log File";
            if(defaultPath != "")
            {
                openFileDialog1.InitialDirectory = defaultPath;
            }

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var sr = new StreamReader(fs))
                {
                    file = sr.ReadToEnd();
                    sr.Close();
                    procesLogFile();
                }

            }
        }

        private void lvLogTable_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) != 0)
            {
                if (e.KeyCode == Keys.C)
                {
                    CopySelectedValuesToClipboard();
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

            Clipboard.SetText(builder.ToString());
        }
    }
}
