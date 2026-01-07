# How To Set This Up For The First Time

## Setting Up Git

### Download Git Off Of [Git's Official Website](https://git-scm.com/install)

<img width="974" height="423" alt="Download Git" src="https://github.com/user-attachments/assets/e54389d7-56b5-4b86-b657-8d3a059313a0" />

**Use the following setup settings**

<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/a55ad9e6-44b2-4bc4-bd56-ab2693a3e0ba" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/b179496b-31e0-49fc-a98d-6344095c744d" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/24edd088-885d-41ad-a8f6-5c68b41f341b" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/37ca8147-894c-4551-b9f9-634133823377" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/fa3e7efe-58c9-454a-8a8d-f3079056ca26" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/e33597d5-6200-407c-96f6-f8a78d1322b5" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/12c68179-c935-40b1-9e20-d569bc791e2f" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/2e9b29fc-acba-4c1f-adc3-f850b313b5a3" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/c1297284-f209-4d3b-9e9e-dca56bb8d8e7" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/b26baae7-c99b-40a0-a4d6-527df1b9a072" />
<img width="400" height="300" alt="image" src="https://github.com/user-attachments/assets/66c2ec4a-8235-449a-a16e-a9b97b3aebb1" />

**Enter Your Git Account Info**

- Open a new CMD and type the following: <br>
`git config --global user.name \[WhateverYourGitHubUsernameIs]` <br>
`git config --global user.email \[WhateverYourGitHubEmailIs]` 

## Get the Unity Project Using Git

**Open CMD and type "git clone https://github.com/ICROSStheLINE/WarTorn.git" to Clone the Repository**

<img width="1000" height="300" alt="Clone Repo" src="https://github.com/user-attachments/assets/1f075f12-0407-46fc-af5f-0847f7828983" />

**Set it up in Unity Hub**

- Open Unity Hub
- Click Add
- Click Add Project From Disk and find "WarTorn" in the File Explorer that just opened
<img width="700" height="300" alt="Add Project to Hub" src="https://github.com/user-attachments/assets/15591091-df1e-4084-bb9b-9017272e2b5b" />

# How to Save Changes You Made (Pushing Commits)

## Using CMD

**Type "cd WarTorn" For CMD to Open That Directory**
<img width="416" height="186" alt="image" src="https://github.com/user-attachments/assets/63f675a1-74ab-43b2-970c-e6deb5354bfb" />

**Type `git add .`**
<img width="416" height="186" alt="image" src="https://github.com/user-attachments/assets/2fa152d0-1ab7-4d32-8317-9b0b74b464ee" />

**Type `git commit -m 'Whatever Your Commit Message Is'`**
<img width="432" height="186" alt="image" src="https://github.com/user-attachments/assets/596d9fb9-6b4a-41b1-9950-96ca7c1e869c" />

**Type `git push`**
<img width="482" height="228" alt="image" src="https://github.com/user-attachments/assets/67bbfa70-e22f-4d64-9d6d-68f54926b95e" />

## Using GitHub Desktop App

**Add the Existing Repo to it**
<img width="1200" height="350" alt="Untitled" src="https://github.com/user-attachments/assets/ee2a7986-5b9a-4487-a703-2af4df2a10d3" />

**Add the Files You Want to Prepare to Save (Commit)**
<img width="500" height="350" alt="Untitled" src="https://github.com/user-attachments/assets/f56306ec-80b5-4a63-8c2e-746a9b8c5c82" />

**Add the Message You Want with the Commit**
<img width="500" height="350" alt="Untitled" src="https://github.com/user-attachments/assets/2fa762fb-274b-4f3d-86a1-e518a9ca11af" />

**Commit and Push**
<img width="500" height="350" alt="image" src="https://github.com/user-attachments/assets/67abd868-35a8-44de-a7a5-be5012194950" />

# How to Receive Your Collaborators' Saved Changes (Pulling Commits)

## Using CMD 

**Type "cd WarTorn" for CMD to Open that Directory**
<img width="416" height="186" alt="image" src="https://github.com/user-attachments/assets/63f675a1-74ab-43b2-970c-e6deb5354bfb" />

**Type `git pull`**
<img width="416" height="186" alt="image" src="https://github.com/user-attachments/assets/98564b90-d610-4285-abac-11885065ad60" />
<br> 
~~~ Note ~~~<br>
Note that it might not properly pull in the changes due to there being a "merge conflict". <br>
This basically means that you and someone else worked on the same thing at the same time and Git doesn't know which one to override over the other. <br>
This means you'll have to do some manual work to try and merge both changes together. <br>
If this happens contact one of the programmers as they have a lot of experience with this kind of thing. <br>
~~~~~~~~~~~~

## Using GitHub Desktop App

**Literally just click this like once or twice**
<img width="928" height="291" alt="image" src="https://github.com/user-attachments/assets/99ef8aad-f968-4219-9a58-4d5ba0c29d07" />





