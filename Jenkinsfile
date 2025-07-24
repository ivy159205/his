pipeline {
    agent any

    environment {
        // Tên image sẽ được build
        DOCKER_IMAGE_NAME = "my-webapp"
        // Tên container sẽ được chạy
        DOCKER_CONTAINER_NAME = "my-webapp-container"
        // Tên network để kết nối app và db
        DOCKER_NETWORK = "app-network"
    }

    stages {
        stage('Setup Network') {
            steps {
                script {
                    // Kiểm tra xem network đã tồn tại chưa, nếu chưa thì tạo mới
                    def existingNetwork = sh(script: "docker network ls --filter name=^${DOCKER_NETWORK}$ --format '{{.Name}}'", returnStdout: true).trim()
                    if (existingNetwork != DOCKER_NETWORK) {
                        echo "Creating Docker network: ${DOCKER_NETWORK}"
                        sh "docker network create ${DOCKER_NETWORK}"
                    } else {
                        echo "Docker network '${DOCKER_NETWORK}' already exists."
                    }

                    // Kết nối SQL Server vào network nếu chưa có
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
                // Sử dụng Dockerfile trong thư mục hiện tại để build image
                script {
                    docker.build(DOCKER_IMAGE_NAME, '.')
                }
            }
        }

        stage('Deploy Application') {
            steps {
                echo "Deploying the new container..."
                script {
                    // Dừng và xóa container cũ nếu nó đang tồn tại
                    def runningContainer = sh(script: "docker ps -a -f name=${DOCKER_CONTAINER_NAME} --format '{{.ID}}'", returnStdout: true).trim()
                    if (runningContainer) {
                        echo "Stopping and removing old container..."
                        sh "docker stop ${DOCKER_CONTAINER_NAME}"
                        sh "docker rm ${DOCKER_CONTAINER_NAME}"
                    }

                    // Chạy container mới từ image vừa build
                    // Quan trọng: Sử dụng --network để kết nối vào cùng mạng với DB
                    // Cung cấp chuỗi kết nối qua biến môi trường
                    sh """
                    docker run -d --name ${DOCKER_CONTAINER_NAME} \
                               --network ${DOCKER_NETWORK} \
                               -p 8083:8080 \
                               -e 'ConnectionStrings__DefaultConnection=Data Source=sql_server_db;Initial Catalog=HIS_Database;User ID=sa;Password=Your_Strong_Password123;TrustServerCertificate=True' \
                               ${DOCKER_IMAGE_NAME}
                    """
                }
            }
        }
    }

    post {
        always {
            // Dọn dẹp workspace sau khi build
            cleanWs()
        }
    }
}