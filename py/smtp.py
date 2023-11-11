# smtp handling
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

# Gmail SMTP settings
smtp_server = 'smtp.gmail.com'
smtp_port = 587  # Use TLS port 587
smtp_username = 'mulemr118@gmail.com'
smtp_password = 'dzsnhhiqfeefqqic'  

# Create the email message
subject = 'Trial Four'
from_email = 'mulemr118@gmail.com'
to_emails = ['emmanuelmulwa06@gmail.com'] 
message = 'Hello, this is trial message four.'

#variables


msg = MIMEMultipart()
msg['From'] = from_email
msg['To'] = ', '.join(to_emails)  # Combine multiple recipients into a single string
msg['Subject'] = subject

msg.attach(MIMEText(message, 'plain'))

# Connect to the Gmail SMTP server
try:
    server = smtplib.SMTP(smtp_server, smtp_port)
    server.starttls()  # Use TLS encryption
    server.login(smtp_username, smtp_password)
    
    # Send the email
    server.sendmail(from_email, to_emails, msg.as_string())
    server.quit()
    
    print('Email sent successfully')
except Exception as e:
    print(f'Email could not be sent. Error: {str(e)}')

