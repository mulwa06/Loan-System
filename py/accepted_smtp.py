import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

#read the text file
with open(r'C:\Users\mule\source\repos\Loan System\text\selected_applicant_details.txt', 'r') as file:
    Timestamp  = file.readline().strip()
    Official_Name  = file.readline().strip()
    Age  = file.readline().strip()
    Sex  = file.readline().strip()
    Government_ID  = file.readline().strip()
    Government_ID_Photo  = file.readline().strip()
    Occupation  = file.readline().strip()
    PaySlipImage  = file.readline().strip()
    LoanDuration  = file.readline().strip()
    LoanAmount  = file.readline().strip()
    PhoneNo  = file.readline().strip()
    Email  = file.readline().strip()
    ApplicantEssay  = file.readline().strip()
    Status  = file.readline().strip()
    Time_of_Update  = file.readline().strip()


# other variables for the message
interest_rate = '5%'

def main():    
    # Gmail SMTP settings
    smtp_server = 'smtp.gmail.com'
    smtp_port = 587  # Use TLS port 587
    smtp_username = 'mulemr118@gmail.com'
    smtp_password = 'dzsnhhiqfeefqqic'

    # Create the email message
    subject = 'Congratulations! Your Loan Application has been Approved'
    from_email = 'mulemr118@gmail.com'
    to_emails = [Email]

    approval_message = f"""
    Dear {Official_Name},

    We are pleased to inform you that your recent loan application has been reviewed and approved. We understand the importance of financial assistance, and we are happy to be able to support you.

    Loan Details:
    - Loan Amount: {LoanAmount}
    - Interest Rate: {interest_rate}
    - Repayment Term: {LoanDuration}

    If you have any questions or need further clarification, feel free to contact our customer support.

    We appreciate your trust in our services and look forward to assisting you in achieving your financial goals.

    Best Regards,
    [Your Company Name]
    [Contact Information]
    """

    msg_denied = MIMEMultipart()
    msg_denied['From'] = from_email
    msg_denied['To'] = ', '.join(to_emails)  # Combine multiple recipients into a single string
    msg_denied['Subject'] = subject

    msg_denied.attach(MIMEText(approval_message, 'plain'))

    # Connect to the Gmail SMTP server
    try:
        server = smtplib.SMTP(smtp_server, smtp_port)
        server.starttls()  # Use TLS encryption
        server.login(smtp_username, smtp_password)

        # Send the email
        server.sendmail(from_email, to_emails, msg_denied.as_string())
        server.quit()

        print('Approval Email sent successfully')
    except Exception as e:
        print(f'Approval Email could not be sent. Error: {str(e)}')

main()

