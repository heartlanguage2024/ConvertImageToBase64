<header>

# Setup Github Actions Step by Step

## **1. Install GitHub CLI (`gh`)**
You need to install **GitHub CLI** to manage workflows and authentication.  

### **Option 1: Install via Winget (Windows)**
1. Open **PowerShell** as Administrator.  
2. Run the following command to install GitHub CLI:  
   ```powershell
   winget install --id GitHub.cli
   ```
3. If prompted with:  
   ```
   すべてのソース契約条件に同意しますか? (Y/N)
   ```
   - Type `Y` and press **Enter**.  

4. Verify the installation:  
   ```powershell
   gh --version
   ```
   If installed successfully, you should see output like:  
   ```
   gh version 2.69.0 (2025-03-26)
   ```

### **Option 2: Install Manually**
- Download the **GitHub CLI Windows installer** from:  
  👉 [https://github.com/cli/cli/releases](https://github.com/cli/cli/releases)  
- Run the `.msi` installer and complete the setup.  

---

## **2. Authenticate GitHub CLI (`gh auth login`)**
After installing, you need to **log in** to GitHub CLI.  

1. In **PowerShell** or **CMD**, run:  
   ```powershell
   gh auth login
   ```
2. You'll be asked:  
   ```
   Authenticate Git with your GitHub credentials? (Y/n)
   ```
   - Type **`Y`** and press **Enter**.  

3. Select the authentication method:  
   ```
   How would you like to authenticate GitHub CLI?
   > Login with a web browser
   > Paste an authentication token
   ```
   - Select **Login with a web browser** (press **Enter**).  

4. Follow the instructions to **authorize GitHub CLI**.  

5. Verify authentication:  
   ```powershell
   gh auth status
   ```
   If successful, you will see:  
   ```
   Logged in to github.com as your-username
   ```

### **(Optional) Disable Authentication**
If you want to **log out** later, run:  
```powershell
gh auth logout
```

---

## **3. Initialize Git Repository & Ignore Unnecessary Files**
1. Open a **terminal (PowerShell, CMD, or Git Bash)**.  
2. Navigate to your project folder:  
   ```powershell
   cd C:\Projects\ConvertImageToBase64
   ```
3. Initialize Git (if not already initialized):  
   ```powershell
   git init
   ```

### **Modify `.gitignore` to Ignore Unwanted Files**
Update your `.gitignore` file (`C:\Projects\ConvertImageToBase64\.gitignore`) to **ignore VS files** and **cache files**:  

```plaintext
# Visual Studio files
.vs/
*.suo
*.user
*.db
*.opendb

# Build output
bin/
obj/
*.exe
*.dll
*.pdb

# OS generated files
Thumbs.db
ehthumbs.db
.DS_Store
```

Commit your changes:  
```powershell
git add .gitignore
git commit -m "Updated .gitignore"
```

---

## **4. Fix Git Add Permission Errors**
If you see an error like:  
```
error: open(".vs/ConvertImageToBase64/FileContentIndex/..."): Permission denied
```
### **Solution**
1. Close **Visual Studio** (VS might be locking the files).  
2. Run in **PowerShell** (to remove the locked files from tracking):  
   ```powershell
   git rm -r --cached .vs/
   ```
3. Add `.vs/` to `.gitignore` and commit:  
   ```powershell
   git add .gitignore
   git commit -m "Ignore .vs/ files"
   ```

---

## **5. Verify & Run GitHub Actions Workflows**
### **Check Available Workflows**
To list all workflows:  
```powershell
gh workflow list
```
If you see multiple workflows like:  
```
0-welcome.yml
1-create-a-workflow.yml
2-add-a-job.yml
...
potato.yml
```

### **Run a Specific Workflow**
If you want to run `potato.yml`:  
```powershell
gh workflow run potato.yml
```

---

## **6. Create a GitHub Actions Workflow (`potato.yml`)**
Inside `.github/workflows/potato.yml`, create a **GitHub Actions workflow** to **build and test** your WPF project:  

```yaml
name: Build and Test WPF App

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main
  workflow_dispatch:  # Allows manual execution

jobs:
  build:
    runs-on: windows-latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Set up .NET SDK
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'  # Adjust based on your project

      - name: Restore dependencies
        run: dotnet restore

      - name: Build the project
        run: dotnet build --configuration Release --no-restore

      - name: Run tests
        run: dotnet test --configuration Release --no-restore --verbosity normal
```

Commit and push this file:  
```powershell
git add .github/workflows/potato.yml
git commit -m "Add GitHub Actions workflow"
git push origin main
```

---

## **7. Verify Workflow Execution**
After pushing, check the **GitHub Actions** tab in your repository:  
👉 **GitHub → Your Repo → Actions**

If you want to **manually trigger** the workflow:  
```powershell
gh workflow run potato.yml
```

---

## **8. Understand Build Steps in GitHub Actions**
By default, GitHub Actions include **built-in steps** like:  

1. **Set up job** ✅ (automatically done by GitHub Actions)  
2. **Checkout code** ✅ (`actions/checkout`)  
3. **Set up .NET SDK** ✅ (`actions/setup-dotnet`)  
4. **Restore dependencies** ✅ (`dotnet restore`)  
5. **Build the project** ✅ (`dotnet build`)  
6. **Run tests** ✅ (`dotnet test`)  
7. **Post Checkout Code** (automatically cleans up)  
8. **Complete job** (marks workflow as finished)  

You **only need to define steps from `Checkout code` onwards** in `potato.yml`.

---

## **Summary of All Steps**
✅ **1. Install GitHub CLI (`gh`)**  
✅ **2. Authenticate (`gh auth login`)**  
✅ **3. Initialize Git & Ignore VS Files (`.gitignore`)**  
✅ **4. Fix Git Errors (`Permission Denied` on .vs/)**  
✅ **5. Check & Run Workflows (`gh workflow list`)**  
✅ **6. Create & Push `potato.yml` Workflow**  
✅ **7. Verify & Debug in GitHub Actions**  
✅ **8. Understand Built-in Steps in GitHub Actions**  

---




# Hello GitHub Actions

_Create and run a GitHub Actions workflow._

</header>

## Step 1: Create a workflow file

_Welcome to "Hello GitHub Actions"! :wave:_

**What is _GitHub Actions_?**: GitHub Actions is a flexible way to automate nearly every aspect of your team's software workflow. You can automate testing, continuously deploy, review code, manage issues and pull requests, and much more. The best part, these workflows are stored as code in your repository and easily shared and reused across teams. To learn more, check out these resources:

- The GitHub Actions feature page, see [GitHub Actions](https://github.com/features/actions).
- The "GitHub Actions" user documentation, see [GitHub Actions](https://docs.github.com/actions).

**What is a _workflow_?**: A workflow is a configurable automated process that will run one or more jobs. Workflows are defined in special files in the `.github/workflows` directory and they execute based on your chosen event. For this exercise, we'll use a `pull_request` event.

- To read more about workflows, jobs, and events, see "[Understanding GitHub Actions](https://docs.github.com/en/actions/learn-github-actions/understanding-github-actions)".
- If you want to learn more about the `pull_request` event before using it, see "[pull_request](https://docs.github.com/en/developers/webhooks-and-events/webhooks/webhook-events-and-payloads#pull_request)".

To get you started, we ran an Actions workflow in your new repository that, among other things, created a branch for you to work in, called `welcome-workflow`.

### :keyboard: Activity: Create a workflow file

1. Open a new browser tab, and navigate to this same repository. Then, work on the steps in your second tab while you read the instructions in this tab.
1. Create a pull request. This will contain all of the changes you'll make throughout this part of the course.

   Click the **Pull Requests** tab, click **New pull request**, set `base: main` and `compare:welcome-workflow`, click **Create pull request**, then click **Create pull request** again.

1. Navigate to the **Code** tab.
1. From the **main** branch dropdown, click on the **welcome-workflow** branch.
1. Navigate to the `.github/workflows/` folder, then select **Add file** and click on **Create new file**.
1. In the **Name your file** field, enter `welcome.yml`.
1. Add the following content to the `welcome.yml` file:

   ```yaml copy
   name: Post welcome comment
   on:
     pull_request:
       types: [opened]
   permissions:
     pull-requests: write
   ```

1. To commit your changes, click **Commit changes**.
1. Type a commit message, select **Commit directly to the welcome-workflow branch** and click **Commit changes**.
1. Wait about 20 seconds, then refresh this page (the one you're following instructions from). A separate Actions workflow in the repository (not the workflow you created) will run and will automatically replace the contents of this README file with instructions for the next step.

<footer>

---

Get help: [Post in our discussion board](https://github.com/orgs/skills/discussions/categories/hello-github-actions) &bull; [Review the GitHub status page](https://www.githubstatus.com/)

&copy; 2023 GitHub &bull; [Code of Conduct](https://www.contributor-covenant.org/version/2/1/code_of_conduct/code_of_conduct.md) &bull; [MIT License](https://gh.io/mit)

</footer>

---

## ✅ **How to Move the Runner to Another PC**
Since you don't have `svc.cmd`, follow these steps:

1. **Unregister the runner from GitHub** (so you can reconfigure it on another PC).  
   Run this command inside the `actions-runner` folder:

   ```powershell
   .\config.cmd remove --token <TOKEN>
   ```

   (Replace `<TOKEN>` with a **removal token** from GitHub → Settings → Actions → Runners → Remove.)

2. **Copy the `actions-runner` folder** to the shared PC.

3. **Set up the runner again** on the shared PC using:

   ```powershell
   .\config.cmd --url https://github.com/<owner>/<repo> --token <TOKEN>
   ```

   Replace `<owner>` with your **GitHub username or organization** and `<repo>` with your **repository name**.

4. **Start the runner manually** on the new PC:

   ```powershell
   .\run.cmd
   ```

5. **(Optional) Install it as a service** on the new PC:

   ```powershell
   .\svc.cmd install
   .\svc.cmd start
   ```
---

## **GitHub Actions Runner does not support UNC paths (`\\SharePCName\04_Share\actions-runner`)** when running `run.cmd`.  

### ✅ **Fix: Use a Mapped Drive or Move to a Local Path**
#### **Option 1: Map the Network Drive and Run Again**
1. **Map the network folder to a drive letter (e.g., `Z:`)**  
   Run this command in **PowerShell or CMD**:
   ```powershell
   net use Z: \\SharePCName\04_Share
   ```
2. **Move to the mapped drive and run the runner**
   ```powershell
   cd Z:\actions-runner
   .\run.cmd
   ```

---

#### **Option 2: Move Runner to a Local Directory**
If mapping the drive does not work, move the **GitHub Actions Runner** to a local folder (e.g., `C:\actions-runner`):

1. **Copy files from the network share to a local directory**
   ```powershell
   robocopy "\\SharePCName\04_Share\actions-runner" "C:\actions-runner" /E
   ```
2. **Run the runner from the local path**
   ```powershell
   cd C:\actions-runner
   .\run.cmd
   ```

---

### 📌 **Why This Happens**
- **Windows CMD does not support UNC paths (`\\server\share\...`) as a working directory.**  
- The error **"Not configured. Run config.(sh/cmd)"** means the runner needs to be configured again.  
- The runner **must be set up in a local path or on a mapped drive** before execution.

---

### 🔧 **Final Steps**
If you moved the runner or mapped a drive, you must **reconfigure it** using:

```powershell
cd C:\actions-runner  # Or Z:\actions-runner
.\config.cmd
```

Then, start it again:

```powershell
.\run.cmd
```

---

## Potato Workflow with ngrok and Jenkins Installed at Github Actions
```yml
name: Potato Workflow

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main

jobs:
  build:
    runs-on: windows-latest

    strategy:
      matrix:
        dotnet-version: [8.0]

    steps:
      - name: Checkout code
        uses: actions/checkout@v2

      - name: Set up .NET SDK
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: ${{ matrix.dotnet-version }}

      - name: Restore dependencies
        run: dotnet restore

      - name: Build the project
        run: dotnet build --configuration Release

      - name: Run tests
        run: dotnet test --configuration Release --no-build --verbosity normal

      - name: Set up Git credentials
        run: |
          git config --global user.name "GitHub Actions"
          git config --global user.email "actions@github.com"

      - name: Fetch and checkout production branch
        run: |
          git fetch origin
          git checkout production

      - name: Merge main into production
        run: |
          git pull origin production

      - name: Push changes to production
        run: |
          git push origin production
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}

      # ✅ Install ngrok using Chocolatey
      - name: Install ngrok
        run: |
          # Ensure Chocolatey is installed (for fresh runners)
          Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
          
          # Install ngrok using Chocolatey
          choco install ngrok -y

      # ✅ Authenticate ngrok with the authtoken
      - name: Authenticate ngrok
        run: |
          ngrok authtoken ${{ secrets.NGROK_AUTHTOKEN }}

      # ✅ Start ngrok and get URL
      - name: Start ngrok and get URL
        run: |
          # Start ngrok to tunnel port 8080
          Start-Process -NoNewWindow -FilePath "ngrok.exe" -ArgumentList "http", "8080"
          
          # Wait for ngrok to establish the tunnel
          Start-Sleep -Seconds 5

          # Get the public ngrok URL from the local ngrok API
          $ngrokUrl = (Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels").tunnels[0].public_url
          Write-Output "ngrok URL: $ngrokUrl"
          echo "NGROK_URL=$ngrokUrl" >> $GITHUB_ENV

      # ✅ Add Jenkins Trigger
      - name: Start ngrok and get public URL
        run: |
          # Start ngrok in the background
          Start-Process -NoNewWindow -FilePath "ngrok" -ArgumentList "http", "192.168.160.1:8080"

          # Wait for ngrok to start (retry every 5 seconds for up to 60 seconds)
          $retries = 12
          $ngrokReady = $false
          while ($retries -gt 0) {
              try {
                  # Try to get the ngrok URL
                  $tunnels = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels"
                  $ngrokUrl = $tunnels.tunnels[0].public_url
                  $ngrokReady = $true
                  Write-Host "Ngrok URL: $ngrokUrl"
                  break
              } catch {
                  Write-Host "Waiting for ngrok to be ready..."
                  Start-Sleep -Seconds 5
                  $retries--
              }
          }

          if (-not $ngrokReady) {
              Write-Host "Ngrok failed to start after multiple retries."
              exit 1
          }

          # Trigger Jenkins job
          $username = 'potato'
          $api_token = '110db95f19398729f40245888ff5f4c220'

          # Combine username and API token, and then base64 encode them
          $credentials = "${username}:${api_token}"
          $base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($credentials))

          # Set headers with base64-encoded credentials
          $headers = @{
              "Authorization" = "Basic $base64AuthInfo"
          }

          # Set the Jenkins job URL
          $jenkinsUrl = "$ngrokUrl/job/potato/build"  # Combine ngrok URL and Jenkins job endpoint

          Write-Host "Triggering Jenkins job at: $jenkinsUrl"

          # Trigger the Jenkins job using the full URL
          Invoke-WebRequest -Uri $jenkinsUrl -Method Post -Headers $headers
        shell: pwsh

```
---

## Potato Workflow with ngrok and Jenkins Installed at Local Machine
```yml
name: Potato Workflow

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main

jobs:
  build:
    runs-on: windows-latest

    strategy:
      matrix:
        dotnet-version: [8.0]

    steps:
      - name: Checkout code
        uses: actions/checkout@v2

      - name: Set up .NET SDK
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: ${{ matrix.dotnet-version }}

      - name: Restore dependencies
        run: dotnet restore

      - name: Build the project
        run: dotnet build --configuration Release

      - name: Run tests
        run: dotnet test --configuration Release --no-build --verbosity normal

      - name: Set up Git credentials
        run: |
          git config --global user.name "GitHub Actions"
          git config --global user.email "actions@github.com"

      - name: Fetch and checkout production branch
        run: |
          git fetch origin
          git checkout production

      - name: Merge main into production
        run: |
          git pull origin production

      - name: Push changes to production
        run: |
          git push origin production
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}

      # ✅ Add Jenkins Trigger
      - name: Trigger Jenkins Job
        run: |
          $username = 'potato'
          $api_token = '110db95f19398729f40245888ff5f4c220'
          
          # Combine username and API token, and then base64 encode them
          $credentials = "${username}:${api_token}"
          $base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($credentials))
          
          # Set headers with base64-encoded credentials
          $headers = @{
              "Authorization" = "Basic $base64AuthInfo"
          }

          # Trigger the Jenkins job
          Invoke-WebRequest -Uri "https://db53-210-139-66-104.ngrok-free.app/job/potato/build" -Method Post -Headers $headers
        shell: pwsh

```
---
# **start_services.bat** to start all the services 

```bat
@echo off
echo Starting Jenkins...

:: Start Jenkins in the background
start /B java -jar jenkins.war --httpPort=8080 --httpListenAddress=0.0.0.0

echo Jenkins started.

:: Wait a few seconds for Jenkins to start
timeout /t 5

:: Check if Actions Runner is running
tasklist | find /i "Runner.Listener.exe" > nul
if %errorlevel% neq 0 (
    echo GitHub Actions Runner is not running. Starting now...
    cd /d "C:\Projects\actions-runner"
    start /B run.cmd
) else (
    echo GitHub Actions Runner is already running.
)

:: Start ngrok and capture the public URL
echo Starting ngrok...
start /B ngrok http 8080 > nul 2>&1

:: Wait for ngrok to initialize
timeout /t 5

:: Fetch the ngrok URL using PowerShell (to handle JSON parsing)
for /f "delims=" %%A in ('powershell -Command "(Invoke-RestMethod -Uri 'http://127.0.0.1:4040/api/tunnels').tunnels[0].public_url"') do set NGROK_URL=%%A

:: Trim the quotation marks from the URL
set NGROK_URL=%NGROK_URL:"=%

echo ngrok URL: %NGROK_URL%

:: Update the ngrok URL in potato.yml using PowerShell
powershell -Command "(Get-Content C:\Projects\ConvertImageToBase64\.github\workflows\potato.yml) -replace 'https://.*?\.ngrok-free\.app', '%NGROK_URL%' | Set-Content C:\Projects\ConvertImageToBase64\.github\workflows\potato.yml"

:: Pull the latest changes from origin and merge with local changes
echo Pulling latest changes from origin main...
cd /d C:\Projects\ConvertImageToBase64
git fetch origin
git pull origin main

:: Check if there was an error while pulling (e.g., merge conflicts)
if %errorlevel% neq 0 (
    echo There was an error while pulling from the remote repository. Please resolve any conflicts and try again.
    pause
    exit /b
)

:: Check if a merge is in progress (MERGE_HEAD exists)
if exist .git\MERGE_HEAD (
    echo Merge is in progress. Committing the merge...
    git commit --no-edit
)

:: Check for uncommitted changes (before merge or after conflict resolution)
git diff --exit-code > nul
if %errorlevel% neq 0 (
    echo Uncommitted changes detected. Committing changes...
    git add .
    git commit -m "Auto-commit changes"
)

:: Push the merged changes to GitHub
git push origin main


echo All services are running. The GitHub Actions workflow has been updated.
pause
```

---

# To build and run your test project (`Fw.Test`), follow these steps: 

 

### **Step 1: Verify the Test Project Type**  
Check if `Fw.Test` is:  
- A **C# test project** (`.csproj` using `xUnit`, `NUnit`, or `MSTest`)  
- A **C++ test project** (`.vcxproj`, likely using Google Test or Boost.Test)  

### **Step 2: Build the Test Project**  
1. **If `Fw.Test` is a C# test project**  
   Run:  
   ```sh
   dotnet build Fw.Test/Fw.Test.csproj
   ```
   or  
   ```sh
   msbuild Fw.Test/Fw.Test.csproj /p:Configuration=Debug /p:Platform=x64
   ```

2. **If `Fw.Test` is a C++ test project**  
   Run:  
   ```sh
   msbuild Fw.sln /p:Configuration=Debug /p:Platform=x64
   ```

### **Step 3: Run the Tests**  
1. **If it's a C# test project**  
   ```sh
   dotnet test Fw.Test/Fw.Test.csproj
   ```
   or  
   ```sh
   vstest.console.exe Fw.Test/bin/Debug/netX.XX/Fw.Test.dll
   ```
   (Replace `netX.XX` with your .NET version, e.g., `net6.0` or `net8.0`.)

2. **If it's a C++ test project**  
   - If using **Google Test**, locate and run the compiled test executable:  
     ```sh
     Fw.Test\x64\Debug\Fw.Test.exe
     ```
   - If using **Boost.Test**, run:  
     ```sh
     Fw.Test\x64\Debug\Fw.Test.exe --run_test=all
     ```

---

## To manage the runner service on Windows:

1. **Stop the Runner Service:**
   You can stop the GitHub Actions runner by using the Windows service management commands.

   Run the following command to stop the runner service:

   ```powershell
   Stop-Service -Name "actions.runner.*" -Force
   ```

   Replace `"actions.runner.*"` with the actual name of your runner service if it is different. You can check the service name by running:

   ```powershell
   Get-Service | Where-Object { $_.DisplayName -like "*actions.runner*" }
   ```

2. **Start the Runner Service:**
   After stopping the service, restart it with:

   ```powershell
   Start-Service -Name "actions.runner.*"
   ```

### If you are not using a service and want to run it manually:
1. **Stop the Runner Process:**
   If you started the runner manually (via `./run.cmd`), you can simply stop it by closing the terminal or using `Ctrl + C`.

2. **Start it Again:**
   To start the runner manually again, use the following command:

   ```powershell
   .\run.cmd
   ```

---
