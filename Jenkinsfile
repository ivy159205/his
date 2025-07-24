pipeline {
    // THAY ĐỔI AGENT Ở ĐÂY
    agent {
        docker {
            image 'docker:26-cli' // Sử dụng image có chứa Docker CLI
            args '-v /var/run/docker.sock:/var/run/docker.sock' // Map socket vào agent này
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

                    sh """
                    if ! docker inspect -f '{{.NetworkSettings.Networks.${DOCKER_NETWORK}}}' sql_server_db > /dev/null 2>&1; then
                        echo "Connecting sql_server_db to ${DOCKER_NETWORK}"
                        docker network connect ${DOCKER_NETWORK} sql_server_db
                    else
                        echo "sql_server_db is already connected to ${DOCKER_NETWORK}"
                    fi
                    """
                }
            }
        }

        stage('Build Docker Image') {
            steps {
                echo "Building the Docker image..."
                // Lệnh docker.build bây giờ không cần thiết vì chúng ta đã có agent là docker
                // Thay vào đó, dùng lệnh sh trực tiếp sẽ rõ ràng hơn
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