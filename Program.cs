using System;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace ClearingLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string logDirectoryPath = @"C:\Logs";  

            // Method to clear the logs
            ClearLogs(logDirectoryPath);
            SendEmail("yourmail@gmail.com", "Plan-IT Logs Cleared", "Plan-IT Logs Cleared for this week");
        }

        static void ClearLogs(string logDirectoryPath)
        {

            try
            {
                DateTime currentDate = DateTime.Now.Date;
                
                if (Directory.Exists(logDirectoryPath))
                {
                    string[] logFiles = Directory.GetFiles(logDirectoryPath, "*.txt");

                    if (logFiles.Length == 0)
                    {
                        Console.WriteLine("No log files found to delete.");
                    }
                    else
                    {
                        foreach (string logFile in logFiles)
                        {
                            try
                            {
                                DateTime fileDateTime = File.GetCreationTime(logFile).Date;
                                //if (fileDateTime != currentDate)
                                //{
                                File.Delete(logFile);
                                Console.WriteLine($"Deleted: {logFile}");
                                //}

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error deleting file {logFile}: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while clearing logs: {ex.Message}");
            }
        }
        static void SendEmail(string recipient, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com") // Replace with your SMTP server
                {
                    Port = 587,
                    Credentials = new NetworkCredential("yourmail@gmail.com", "yourpassword"), // Give your password
                    EnableSsl = true 
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("yourmail@gmail.com"),
                    Subject = subject,
                    Body = body,
                };

                mailMessage.To.Add(recipient); 

                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                Console.WriteLine($"Stack Trace: {smtpEx.StackTrace}");
                Console.WriteLine($"Inner Exception: {smtpEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}