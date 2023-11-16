import numpy
import gspread
from google.oauth2 import service_account
from oauth2client.service_account import ServiceAccountCredentials


scope = [
"https://www.googleapis.com/auth/drive", \
"https://www.googleapis.com/auth/drive.file", \
"https://www.googleapis.com/auth/drive.readonly", \
"https://www.googleapis.com/auth/spreadsheets", \
"https://www.googleapis.com/auth/spreadsheets.readonly"	]

# Notes
"""
	The print statements are not necessary and serve to show the progress of the function.
	Various errors may be encountered when network connectivity is unstable. Ensure it's stable. TransportError, ConnectionError are some of the errors
	The numpy package is due to it's efficiency as compared to python lists. Feel free to change it
	The spreadsheet_id will still need to be changed to the relevant one.
	'update_cell', line 35 for me should be retained as True or removed to change a single line
	'Value', line 36 for me needs to be changed to 'Status' or whatever you need to change.
"""

def Update(customer_file = r"C:\Users\mule\source\repos\Loan System\text\selected_applicant_details.txt", key = "Timestamp", update_cell = True):
    # The lines in download database that setup the google sheets we're working on
    spreadsheet_id = "14gCKW6p4FY3Ni57QkknUBtGmTVNkX9tWjxCzQpCfZJ0"
    creds = ServiceAccountCredentials.from_json_keyfile_name(r"C:\Users\mule\source\repos\Loan System\token\token.json", scope)
    client = gspread.authorize(creds)
    sheet = client.open_by_key(spreadsheet_id)
    worksheet = sheet.get_worksheet(0)
    data = numpy.array(worksheet.get_all_values())


    # Now we update the sheets
    update_cell = True
    value = 'STATUS'
    # Dynamic assignment of columns. This version is never out of date
    keys = data[0] 
    values = range(len(keys))
    user_details = dict(zip(keys, values))
    # --- Parse the customer info
    def customer__identifier(file):
        customer_details_file = open(file, 'r')
        file_data =  list( customer_details_file.readlines() )
        customer_details_file.close()

        # Assuming it takes the format
        # Timestamp\n Name\n Age\n Sex\n ...
        customer_details = [n[:-1] for n in file_data]
        identifier = customer_details[user_details[key]]
        if update_cell:
            new_value = customer_details[user_details[value]]
        else:
            new_value = customer_details

        return identifier, new_value

    # --- Find where we need to update
    identifier, new_value = customer__identifier(customer_file)  # The unique value to use to find the customer. The new value we update to 
    # --- --- The column we're searching
    column = data[:, user_details[key] ]
    customer_row, row = 0, -1
    for m in column:
        row += 1
        if m == identifier:
            customer_row = row
    if customer_row == 0: # Given row 0 contains the column titles, Index cannot remain 0 if the unique identifier exists
        msg = f"Customer not found.\nAssertain that the key -{key}- is in the number {user_details[key] + 1}- spot in the file containing the customer details.\nAlso confirm that the customer is indeed an applicant."
        raise ValueError(msg)

    if update_cell:
        # --- --- Construct the cell id, then update the file
        cell_number = chr(65 + user_details[value]) + str(customer_row + 1) # Add one because google sheets rows start from 1
        worksheet.update(cell_number, new_value)

    else: # Untested. Doesn't work with empty strings, Invalid date formats
        identifier, new_row = customer__identifier(customer_file)
        worksheet.update(str(customer_row + 1), new_row)

    # --------- Code to verify update. Not to be included in the final draft ---------
    if update_cell:
        update_type = 'Cell '
    else:
        update_type = "Row "
    print(update_type + "Update made... Waiting for server to update")
    
    print( numpy.array( worksheet.get_all_values() ) )

Update()

