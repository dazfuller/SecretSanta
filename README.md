# The Secret Santa App

This was the result of an evening needing distraction and trying to work out when everyone would be in the office to do the Secret Santa hat pick.

It's a simple console application which reads in a list of participants with names and email addresses, randomly assigns a recipient to each secret santa and then emails each secret santa to let them know who they're buying a gift for.

Name | Email
---- | -----
Robert Banner | rbanner@example.com
Wanda Maximoff | wmaximoff@example.com
Natalia Romanova | nromanova@example.com
Jim Hammond | jhammond@example.com
Carol Danvers | cdanvers@example.com

For each secret santa the app creates a pool of recipients, this pool is made up of everyone in the list with the those who have already been picked and the secret santa themselves being removed, the list is then shuffled to ensure that the order of names in the CSV does not influence the result. If at the end of the process the app is left in a position where a person would pick themselves then the process is restarted and repeats until everyone has a recipient assigned to them.

Once all secret santas have a recipient then an email message is generated, using the template defined in the configuration file, and sent to each santa. No record of the process is written locally to prevent the person running the application knowing who is assigned to whom.

## Running the application

The app is a command line application and is executed as follows

```cmd
C:\repos\secret-santa> dotnet run -i <path to csv>
```

The application will check to see if the file specified exists before running.

## Configuration

An `appsettings.json` file needs to be generated and filled in for the application to run. A template file is provided with the solution which can be saved as `appsettings.json` and have the settings changed. The app assumes that an SMTP server which requires credentials and uses TLS is being used.

## Email message

The email message is stored in `content/message.html`, this can be changed in the `appsettings.json` file. The content of the file is HTML which accepts a placeholder for the recipient name. The placeholder value is `{{name}}`.