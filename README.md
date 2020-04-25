# Folder Cleanup

Cleans up messy folders to other folders! Hoera! Use this as a crontab or scheduled task to ensure always having clean folders

## Configuration

Create a `config.json` inside `Lenimal.FolderCleanup` and provide add the following:

```
{
    "Folders": [
        {
            "Source": "{{folder to be cleaned}}",
            "Destination": "{{folder where new folders will be created}}",
            "Format": {
                "Code": [
                    ".cs",
                    ".css",
                    ".scss",
                    ".js",
                    ".py"
                ],
                "App": [
                    ".dmg"
                ],
                "Images" : [
                    ".iso"
                ],
                "Dev": [
                    ".txt",
                    ".log",
                    ".md",
                    ".yaml",
                    ".json",
                    ".sh",
                    ".crt"
                ],
                "Documents": [
                    ".pdf",
                    ".csv",
                    ".pages",
                    ".key",
                    ".xlsx",
                    ".numbers",
                    ".doc",
                    ".pptx",
                    ".docx"
                ],
                "Compressed": [
                    ".zip",
                    ".rar"
                ],
                "Pictures": [
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".ico"
                ],
                "Folders" : ["__folder__"]
            }
        }
    ]
}
```

## Crontab example

Run every day at 9 am (only works when computer is on, so arriving at office around 8:30 ensures my day starts fresh):

`0 9 * * *  cd /{{path to folder-cleanup}}/Lenimal.FolderCleanup && dotnet run`