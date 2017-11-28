#include <stdio.h>
#include <stdlib.h>
#include <error.h>
#include <string.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <sys/socket.h>
#include <sys/wait.h>

#define SERV_COM_PORT 21
#define SERV_DATA_PORT 20
#define MAXSIZE 1024
#define PORT_MODE 1
#define PASV_MODE 2

int login();
void trimStr(char *str);
void validateArgs(int argc,char* argv[]);
void ftp_mode(int newMode);
int getDataSocket();
int getFile(char *local_file);
int file_copy(int srcfd,int destfd,int *psize);
void ftp_list();
void ftp_cd(char *dir);
void ftp_back();
void ftp_pwd();
void ftp_quit();
void ftp_download(char *remote_file,char *local_file);
void ftp_upload(char* local_file, char* remote_file);
int random_local_port();

int sockfd;
char *serverAddr;
int workMode=PASV_MODE;
struct hostent *host;

int main(int argc,char* argv[]){
    //接收FTP服务器地址
    validateArgs(argc,argv);
    login();
    char *ptr;
    char *argu1;
    char *argu2;
    while(true){
        printf("ftp>");
        char cmd[MAXSIZE];
        scanf("%s",cmd);
        trimStr(cmd);
        if(strncmp(cmd,"pwd",3)==0){
            ftp_pwd();
        }
        else if(strcmp(cmd,"list",4)==0){
            ftp_list();
        }
        else if(strcmp(cmd,"back",4)==0){
            ftp_back();
        }
        else if(strcmp(cmd,"quit",4)==0){
            ftp_quit();
        }
        else if(strcmp(cmd,"pasv",4)==0){
            ftp_mode(PASV_MODE);
        }
        else if(strcmp(cmd,"port",4)==0){
            ftp_mode(PORT_MODE);
        }
        else if(strcmp(cmd,"cd",2)==0){
            cmd+=2;
            trimStr(cmd);
            ftp_cd(cmd);
        }
        else if(strcmp(cmd,"download",8)==0){
            cmd+=8;
            trimStr(cmd);
            ptr=strchr(cmd,' ');
            if(ptr){
                *ptr='\0';
                ftp_download(cmd,trimStr(ptr+1));
            }
        }
        else if(strcmp(cmd,"upload",6)==0){
            cmd+=6;
            trimStr(cmd);
            ptr=strchr(cmd,' ');
            if(ptr){
                *ptr='\0';
                ftp_upload(cmd,trimStr(ptr+1));
            }
        }
        else{
            printf("支持的命令：pwd,list,back,quit,pasv,port,cd,download,upload");
        }
        
    }
    
}

//去掉命令前的空格和末尾空格
void trimStr(char *str){
    while(*(str)=='')
        str++;
    char * ptr;
    ptr=strrchr(str,'\n');
    if(ptr)
        *ptr='\0';
}


//检查启动客户端时的命令
void validateArgs(int argc,char* argv[]){
    switch(argc){
        case 2:
            serverAddr=argv[1];
            break;
        default:
            printf("请输入FTP服务器的IP或者域名\n")；
            exit(1);
            break;
    }
}

//流程:创建TCP套接字，连接服务端21号端口，按照FTP协议，建立命令链路
void login(){
    char buffer[MAXSIZE];
    struct sockaddr_in server_addr;
    int numbytes;
    
    if((host=gethostbyname(serverAddr))==NULL){
        printf("无法访问服务器: %s\n",serverAddr);
		exit(1);
    }
    
    /* if((portnumber=atoi(argv[2]))<0)
    {
        fprintf(stderr,"Usage:%s hostname portnumber\a\n",argv[0]);
        exit(1);
    } */
    
    if((sockfd=socket(AF_INET,SOCK_STREAM,0))==-1){
        printf("socket() 失败\n");
		exit();
    }
    
    bzero(&server_addr,sizeof(server_addr));
    server_addr.sin_family=AF_INET;
    server_addr.sin_port=htons(SERV_COM_PORT);
    memcpy(&server.sin_addr,host->h_addr_list[0],host->h_length);
    
    if(connect(sockfd,(struct sockaddr*)(&server_addr),sizeof(struct sockaddr))==-1){
        printf("与服务器21端口建立连接失败");
        exit(1);
    }
    
    while(true){
        if((numbytes=read(sockfd,buffer,MAXSIZE)==-1){
            printf("read error1");
            exit(1);
        }
        buffer[numbytes]="\0";
        
        //接收到FTP服务发送的欢迎信息，发送用户名
        if(strncmp(buffer,"220",3)==0){
            printf("%s\n",buffer);
            buffer="USER anonymous";
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("发送用户名失败\n");
                exit(1);
            }
            printf("%s\n",buffer);
        }
        //接收到FTP服务器发送的 用户名存在并且是合法用户的信息，发送密码
        else if(strncmp(buffer,"331",3)==0){
            printf("%s\n",buffer);
            buffer="PASS ttluze@gmail.com";
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("发送密码失败\n");
                exit(1);
            }
        }
        //接收到服务器发送的 用户名与密码正确的信息
        else if(strncmp(buffer,"230",3)==0){
            printf("登录FTP服务器成功\n");
            break;
        }
    }
}

//选择FTP的工作模式
//PORT,需要传输数据时，客户端通过21端口发送PORT命令，命令中包含客户端接收数据的端口
//服务器的20号端口与之连接，客户端收到200时表示数据链路建立完成
//PASV，需要传输数据时，客户端通过21端口发送PASV命令，接收的数据中包含227与服务器数据链//路端口号，从客户端发起连接
void ftp_mode(int newMode){
    workMode=newMode;
}

//注意这个里面的错误是否要exit(1)
int getDataSocket(){
    char buffer[MAXSIZE];
    int numbytes;
    int data_sockfd;
    
    switch(workMode){
        PORT_MODE:
            //client_addr是拿来bind套接字，local_addr是拿来获取本地ip
            struct sockaddr_in client_addr,local_addr;
            int addr_len=sizeof(struct sockaddr_in);
            int ret;
            char local_ip[24];
            char buffer[MAXSIZE];
            int listenSockfd;
            int i=0;
            
            if((listenSockfd=socket(AF_INET,SOCK_STREAM,0))==-1){
                printf("PORT创建套接字失败\n");
            }
            
            //PORT命令发送给服务器客户端的ip和端口(数据链路)
            int clientPort=random_local_port();
            bzero(&client_addr,sizeof(client_addr));
            client_addr.sin_family=AF_INET;
            client_addr.sin_port=htons(clientPort);
            client_addr.sin_addr.s_addr=INADDR_ANY;
            bzero(&local_addr, sizeof(struct sockaddr_in));
            
            while(1){
                if(bind(listenSockfd, (struct sockaddr *)(&client_addr), sizeof(struct sockaddr))==-1){
                    clientPort=random_local_port();
                    client_addr.sin_port=htons(clientPort);
                    continue;
                }
                break;
            }
            if(listen(listenSockfd,1)==-1){
                printf("监听客户端data_socket失败\n");
                exit(1);
            }
            //get local host's ip
            if(getsockname(sockfd,(struct sockaddr*)&localaddr,&ret) < 0){
                printf("获取本地IP失败\n");
                exit(1);
            }
                
            snprintf(local_ip, sizeof(local_ip), inet_ntoa(localaddr.sin_addr));
            //change the format to the PORT command needs.
            local_ip[strlen(local_ip)]='\0';
            ip_1 = local_ip;
            ip_2 = strchr(local_ip, '.');
            *ip_2 = '\0';
            ip_2++;
            ip_3 = strchr(ip_2, '.');
            *ip_3 = '\0';
            ip_3++;
            ip_4 = strchr(ip_3, '.');
            *ip_4 = '\0';
            ip_4++;
            snprintf(buffer, sizeof(buffer), "PORT %s,%s,%s,%s,%d,%d",ip_1, ip_2, ip_3, ip_4,	clientPort >> 8, clientPort&0xff);
                
            //发送PORT命令
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("发送指令PORT失败\n");
                exit(1);
            }
            printf("%s",buffer);
                
            //接收服务器返回的信息
            if((numbytes=read(sockfd,buffer,MAXSIZE)==-1){
                printf("read error getDataSocket");
                exit(1);
            }
            buffer[numbytes]="\0";
            
            if(strncmp(buffer,"200",3)==-1){
                printf("指令PORT没有返回200");
                exit(1);
            }
            printf("%s\n",buffer);

            //成功接收到PORT200，接受来自服务端的连接
            while(i < 3){
                //这里的client_addr表示FTP服务器的addr
                if((data_sockfd=accept(listenSockfd, (struct sockaddr *)(&client_addr), &numbytes))==-1){
                    printf("accept error%d\n");
                    i++;
                    continue;
                }
                else 
                    break;
            }
            if(data_sockfd == -1){
                printf("Sorry, you can't use PORT mode. There is something wrong when the server connect to you.\n");
                exit(1);
            }
            close(listenSockfd);
            break;
        
        PASV_MODE:
            //发送PASV
            struct sockaddr_in serv_addr;
            buffer="PASV\r\n";
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("发送指令PASV失败\n");
                exit(1);
            }
            printf("%s",buffer);
            
            //接收服务器返回的信息
            if((numbytes=read(sockfd,buffer,MAXSIZE)==-1){
                printf("read error getDataSocket");
                exit(1);
            }
            buffer[numbytes]="\0";
            
            if(strncmp(buffer,"227",3)==-1){
                printf("指令PASV没有返回227");
                exit(1);
            }
            printf("%s\n",buffer);
            
            //获取服务端的数据端口，从buffer中读取(192,168,1,1,x,y)
            //端口为x*256+y
            //strrchr函数为获得字符最后一次出现的位置的指针
            int serverPort;
            char* buf_ptr;
            buf_ptr=strrchr(buffer,')');
            *buf_ptr='\0';
            buf_ptr = strrchr(buffer, ',');
            serverPort = atoi(buf_ptr + 1);
            *buf_ptr = '\0';
            buf_ptr = strrchr(buffer, ',');
            serverPort += atoi(buf_ptr + 1) * 256;
            
            //创建套接字，与服务端相连
            if((data_sockfd=socket(AF_INET,SOCK_STREAM,0))==-1){
                printf("创建套接字失败,getdatasocket\n");
                exit(1);
            }
            bzero(&server_addr,sizeof(server_addr));
            server_addr.sin_family=AF_INET;
            serv_addr.sin_port=htons(serverPort);
            memcpy(&server.sin_addr,host->h_addr_list[0],host->h_length);
            
            if(connect(data_sockfd,(struct sockaddr*)(&server_addr),sizeof(struct sockaddr))==-1){
                printf("PASV模式建立数据链路失败\n");
                exit(1);
            }
            break;
        default:
            printf("FTP工作模式只能是PORT或者PASV");
            exit(1);
    }
    return data_sockfd;
}

//获取文件操作符
//返回值为-1时表示创建失败,返回值为-2表示用户取消操作
int getFile(char *local_file){
    int filefd = -1;
	struct stat* buf;
	char usr_input;
    
	if (0 == access(local_file, F_OK)){//判断文件是否存在
        //若文件已存在
		buf = (struct stat*)malloc(sizeof(struct stat));
		stat(local_file, buf);//把文件状态读入buf
		if (S_ISDIR(buf->st_mode)){
			printf("%s 是一个目录\n", local_file);
			free(buf);
			return -1;
		}
        
        printf("%s文件已存在，选择覆盖(1)或取消下载(2)操作\n",local_path);
        usr_input=(char)getchar();
        if(user_input=='2'){
            return -2;
        }
        else{
            //删除原文件
            filefd = open(local_file, O_TRUNC | O_WRONLY);
        }
        //下面的代码在FTP服务器支持断点续传时
        
		/* file_size = (long)buf->st_size;
		free(buf);
		remote_file_size = get_remote_file_size(remote_file, sock_control);
		if (file_size < remote_file_size){
			printf("%s with %ld bytes is existed, abort(a)/delete(d)/resume(r)? enter for d:", local_file, file_size);
		}
		else{
			printf("%s with %ld bytes is existed, abort(a)/delete(d)? enter for d:", local_file, file_size);
		}
		usr_input = (char)getchar();
		if (usr_input == 'a')	return (0);
		else if (usr_input == 'r' && file_size < remote_file_size){
			sprintf(rest_cmd, "REST %ld\r\n", file_size);
			ret_value = send_command(rest_cmd, NULL, sock_control);
			if (ret_value == 350){
				filefd = open(local_file, O_APPEND | O_WRONLY);
//				printf("file is append\n");
			}
			else{
				filefd = open(local_file, O_TRUNC | O_WRONLY);
//				printf("file is delete and create new\n");
			}
		}
		else{
			filefd = open(local_file, O_TRUNC | O_WRONLY);
//			printf("file is delete and create new\n");
		} */
	}
	else{
        //若文件不存在
		filefd = open(local_file, O_CREAT | O_WRONLY, S_IRUSR|S_IWUSR | S_IRGRP | S_IROTH);
	}
	if (filefd==-1){
		return -1;
	}
    return filefd;
}

//传输数据，从srcfd到destfd
int file_copy(int srcfd,int destfd,int *psize){
    char* buf;
	char* ptr;
	int read_size = 0;//从服务器一次read读到的长度
	int write_size = 0;//往文件里一次write写的长度
	*psize = 0;
	if (-1 == srcfd || -1 == destfd)
	{
		fprintf(stderr, "fd error");
		return -1;
	}
	buf = (char*)malloc(sizeof(char) * MAXSIZE);
	memset(buf, 0, sizeof(char) * MAXSIZE);
	while (0 != (read_size = read(srcfd, buf, sizeof(char) * MAXSIZE)))
	{
		if (read_size == -1)
		{
			break;
		}
		else if (read_size > 0)
		{
			ptr = buf;
			while (0 != (write_size = write(destfd, ptr, read_size)))
			{
//				printf("file_copy: write_size: %d\n", write_size);
				*psize += write_size;
				if (write_size == -1)
				{
					break;
				}
				else if (write_size == read_size)
				{
					break;
				}
				else if (write_size > 0)
				{
					ptr += write_size;
					read_size -= write_size;
				}
			}
			if (write_size == -1)
			{
				break;
			}
		}
	}
	free(buf);
	return 0;
}

//获取服务器文件列表
//LIST
void ftp_list(){
    char buffer[MAXSIZE];
    int numbytes;
    int data_sockfd=getDataSocket();
    
    buffer="LIST/r/n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送LIST指令失败\n");
        close(data_sockfd);
        return;
    }
    printf("%s",buffer);
    
    //接收服务返回的信息
    if(numbytes=read(data_sockfd,buffer,MAXSIZE)==-1){
        printf("发送LIST指令后，接收服务返回的信息失败\n");
        close(data_sockfd);
        return;
    }
    buffer[numbytes]="\0";
    if(strncmp(buffer,"226",3)==-1){
        printf("指令LIST没有返回226\n");
        close(data_sockfd);
        return;
    }
    printf("%s\n",buffer);
    close(data_sockfd);
    return;
}

//目录操作
//CWD,无数据连接操作，只通过sockfd传输数据
void ftp_cd(char *dir){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="CWD ";
    strcat(buffer,dir);
    strcat(buffer,"\r\n");
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送CWD指令失败\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("发送CWD指令后，接收服务器返回信息失败\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"250",3)==-1){
        printf("指令CWD没有返回250\n");
        return;
    }
    printf("%s\n",buffer);
    return;
}

//目录操作 CDUP
//返回上一级
void ftp_back(){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="CDUP\r\n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送CDUP指令失败\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("发送CDUP指令后，接收服务器返回信息失败\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"250",3)==-1){
        printf("指令CDUP没有返回250\n");
        return;
    }
    printf("%s\n",buffer);
    return;
}

//返回当前工作目录
//PWD
void ftp_pwd(){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="PWD\r\n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送PWD指令失败\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("发送PWD指令后，接收服务器返回信息失败\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"257",3)==-1){
        printf("指令PWD没有返回250\n");
        return;
    }
    printf("%s\n",buffer);
    return;
}

//创建目录
//CMD
void ftp_mkdir(){
    
}

//退出登录
//QUIT
void ftp_quit(){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="QUIT\r\n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送QUIT指令失败\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("发送QUIT指令后，接收服务器返回信息失败\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"221",3)==-1){
        printf("指令QUIT没有返回250\n");
        return;
    }
    printf("%s\n",buffer);
    close(sockfd);
    return;
}

//上传下载
//sockfd发送命令RETR
//data_sockfd接收文件
void ftp_download(char *remote_file,char *local_file){
    int filefd;
    int numbytes;
    if((filefd=getFile(local_file))<0){
        return;
    }
    int data_sockfd=getDataSocket();
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="RETR ";
    strcat(buffer,remote_file);
    strcat(buffer,"/r/n");
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送RETR指令失败\n");
        close(data_sockfd);
        return;
    }
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("发送RETR指令后，接收服务器返回信息失败\n");
        close(data_sockfd);
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"400",3)==-1){
        printf("指令RETR没有返回400\n");
        close(data_sockfd);
        return;
    }
    //命令套接字已经发送命令，且已经接受到服务器返回的成功信息，接着在数据套接字中获取数据
    if(file_copy(data_sockfd,filefd,&numbytes)==-1){
        printf("下载失败\n");
        return;
    }
    close(filefd);
    close(data_sockfd);
    printf("下载成功，接收数据为%d\n",numbytes);
    return;
}


//上传下载
//sockfd发送命令RETR
//data_sockfd接收文件
void ftp_upload(char* local_file, char* remote_file){
    int filefd;
    int numbytes;
    
    if((filefd=open(local_file, O_RDONLY))==-1)
	{
		printf("%s文件打开失败\n",local_file);
		return;
	}
    
    int data_sockfd=getDataSocket();
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="STOR ";
    strcat(buffer,remote_file);
    strcat(buffer,"/r/n");
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("发送STOR指令失败\n");
        close(data_sockfd);
        return;
    }
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("发送STOR指令后，接收服务器返回信息失败\n");
        close(data_sockfd);
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"150",3)==-1){
        printf("指令STOR没有返回150\n");
        close(data_sockfd);
        return;
    }
    //命令套接字已经发送命令，且已经接受到服务器返回的成功信息，接着在数据套接字中获取数据
    if(file_copy(filefd,data_sockfd,&numbytes)==-1){
        printf("上传失败\n");
        return;
    }
    close(filefd);
    close(data_sockfd);
    printf("上传成功，上传数据为%d\n",numbytes);
    return;
}

//随机获取本机一个端口
int random_local_port(){
	int local_port;
	srand((unsigned)time(NULL));
	local_port = rand() % 40000 + 1025;
	return local_port;
}

