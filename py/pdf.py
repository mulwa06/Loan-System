import gspread
from google.oauth2 import service_account
from fpdf import FPDF


# Define the scope and credentials
scope = [
"https://www.googleapis.com/auth/drive", \
"https://www.googleapis.com/auth/drive.file", \
"https://www.googleapis.com/auth/drive.readonly", \
"https://www.googleapis.com/auth/spreadsheets", \
"https://www.googleapis.com/auth/spreadsheets.readonly"	]

def download_sheet_as_pdf(spreadsheet, file_path):
    # Open the specified sheet
    worksheet = spreadsheet.get_worksheet(0)

    # Get all values from the sheet
    values = worksheet.get_all_values()

    # Create a PDF document
    pdf = FPDF(orientation='L', unit='mm', format='A4')
    pdf.set_auto_page_break(auto=True, margin=10)
    pdf.add_page()

    # Set font
    pdf.set_font("Arial", size=6)  # You can change the font and size as needed
    cell_width = 20  # Adjust the cell width as needed

    pdf.set_right_margin(10)  # Adjust the right margin as needed
    pdf.set_left_margin(10)
    
    # Add data from the sheet to the PDF
    if values:
        for row in values:
            for col in row:
                pdf.cell(cell_width, 10, col, 1)
            pdf.ln()

    # Output the PDF to the specified file path
    pdf.output(file_path)

def main():
    # Open the Google Spreadsheet by its title or URL
    spreadsheet_id = "14gCKW6p4FY3Ni57QkknUBtGmTVNkX9tWjxCzQpCfZJ0"
    creds = service_account.Credentials.from_service_account_file(r"C:\Users\mule\source\repos\Loan System\token\token.json", scopes = scope)
    # Authenticate with the Google Sheets API
    client = gspread.authorize(creds) # Error may mean Json file lacks a specific needed Key/Value pair
    # Open the Google Sheet using its ID
    spreadsheet = client.open_by_key(spreadsheet_id)

    # Specify the path where you want to save the PDF
    pdf_path = 'Downloads\spreadsheet.pdf'

    download_sheet_as_pdf(spreadsheet, pdf_path)

main()
