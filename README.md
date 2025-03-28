<header>

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
