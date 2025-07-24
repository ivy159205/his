pipeline {
    agent {
        docker {
            image 'docker:26-cli'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
            user 'root' // Chạy các bước với quyền root bên trong agent
        }
    }

    environment {
        DOCKER_IMAGE_NAME = "my-webapp"
        DOCKER_CONTAINER_NAME = "my-webapp-container"
        DOCKER_NETWORK = "app-network"
    }

    stages {
        stage('Setup Network') {
            steps {
                script {
                    def existingNetwork = sh(script: "docker network ls --filter name=^${DOCKER_NETWORK}\$ --format '{{.Name}}'", returnStdout: true).trim()
                    
                    if (existingNetwork != DOCKER_NETWORK) {
                        echo "Creating Docker network: ${DOCKER_NETWORK}"
                        sh "docker network create ${DOCKER_NETWORK}"
                    } else {
                        echo "Docker network '${DOCKER_NETWORK}' already exists."
                    }

                    // Kiểm tra xem sql_server_db có tồn tại không trước khi connect
                    def sqlServerExists = sh(script: "docker ps -q -f name=sql_server_db", returnStdout: true).trim()
                    if (sqlServerExists) {
                        sh """
                        if ! docker inspect -f '{{.NetworkSettings.Networks.${DOCKER_NETWORK}}}' sql_server_db > /dev/null 2>&1; then
                            echo "Connecting sql_server_db to ${DOCKER_NETWORK}"
                            docker network connect ${DOCKER_NETWORK} sql_server_db
                        else
                            echo "sql_server_db is already connected to ${DOCKER_NETWORK}"
                        fi
                        """
                    } else {
                        echo "Warning: sql_server_db container not found. Skipping network connection."
                    }
                }
            }
        }

        stage('Build Docker Image') {
            steps {
                echo "Building the Docker image..."
                sh "docker build -t ${DOCKER_IMAGE_NAME} ."
            }
        }

        stage('Deploy Application') {
            steps {
                echo "Deploying the new container..."
                script {
                    def runningContainer = sh(script: "docker ps -a -f name=${DOCKER_CONTAINER_NAME} --format '{{.ID}}'", returnStdout: true).trim()
                    if (runningContainer) {
                        echo "Stopping and removing old container..."
                        sh "docker stop ${DOCKER_CONTAINER_NAME}"
                        sh "docker rm ${DOCKER_CONTAINER_NAME}"
                    }

                    sh """
                    docker run -d --name ${DOCKER_CONTAINER_NAME} \
                               --network ${DOCKER_NETWORK} \
                               -p 8081:8080 \
                               -e 'ConnectionStrings__DefaultConnection=Data Source=sql_server_db;Initial Catalog=HIS_Database;User ID=sa;Password=Your_Strong_Password123;TrustServerCertificate=True' \
                               ${DOCKER_IMAGE_NAME}
                    """
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}