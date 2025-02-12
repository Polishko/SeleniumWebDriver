pipeline {
    agent any

    stages {
        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build Project') {
            steps {
                sh 'dotnet build --no-restore' // Avoid duplicate restore
            }
        }

        stage('Run Dotnet Tests') {
            steps {
                sh 'dotnet test --no-restore --logger trx --results-directory HouseRentingSystem.Tests/TestResults'
            }
        }
    }
}
