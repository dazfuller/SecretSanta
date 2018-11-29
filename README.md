# The Secret Santa App

This was the result of an evening needing distraction and trying to work out when everyone would be in the office to do the Secret Santa hat pick.

It's a simple console application which reads in a list of participants with names and email addresses, randomly assigns a recipient to each secret santa and then emails each secret santa to let them know who they're buying a gift for.

For each secret santa the app creates a pool of recipients, this pool is made up of everyone in the list with the those who have already been picked and the secret santa themselves being removed, the list is then shuffled to ensure that the order of names in the CSV does not influence the result. If at the end of the process the app is left in a position where a person would pick themselves then the process is restarted and repeats until everyone has a recipient assigned to them.

Once all secret santas have a recipient then an email message is generated, using the template defined in the configuration file, and sent to each santa. No record of the process is written locally to prevent the person running the application knowing who is assigned to whom.

## Installing the application

The application can be installed on the command line as follows.

```cmd
C:\> dotnet tool install -g SecretSanta
```

## Running the application

The command line application is executed as follows, note that the help information can be displayed at any time using the command is illustrated below.

```cmd
C:\> SecretSant -h

Usage - SecretSanta -options

GlobalOption        Description
Help (-h)           Displays the help information
SmtpServer (-s)     The address of the SMTP server with optional port (e.g. smtp.mailtrap.io:465), default port is 465
SmtpUsername (-u)   User for the SMTP server
SmtpPassword (-p)   Password for the SMTP server
FromAddress (-f)    The email address of the sender
InputPath (-i)      Path to the input CSV file containing participants and their email addresses
MessagePath (-m)    Path to the message HTML file
```

The application will check to see if the files specified exists before running.

An example run would look as follows.

```cmd
C:\> SecretSanta -s "smtp.mailtrap.io:2525" -u <username> -p <password> -f "secretsanta@example.com" -m message.html -i test.csv
```

### Email message

An exmaple email message is stored in `content/message.html`. The content of the file is HTML which accepts a placeholder for the recipient name. The placeholder value is `{{name}}`.

### Input CSV

The following is example content for a CSV file. You can create your own with the same structure and column names to run the application.


```csv
Name,Email
Robert Banner,rbanner@example.com
Wanda Maximoff,wmaximoff@example.com
Natalia Romanova,nromanova@example.com
Jim Hammond,jhammond@example.com
Carol Danvers,cdanvers@example.com
```