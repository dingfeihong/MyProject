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
    //����FTP��������ַ
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
            printf("֧�ֵ����pwd,list,back,quit,pasv,port,cd,download,upload");
        }
        
    }
    
}

//ȥ������ǰ�Ŀո��ĩβ�ո�
void trimStr(char *str){
    while(*(str)=='')
        str++;
    char * ptr;
    ptr=strrchr(str,'\n');
    if(ptr)
        *ptr='\0';
}


//��������ͻ���ʱ������
void validateArgs(int argc,char* argv[]){
    switch(argc){
        case 2:
            serverAddr=argv[1];
            break;
        default:
            printf("������FTP��������IP��������\n")��
            exit(1);
            break;
    }
}

//����:����TCP�׽��֣����ӷ����21�Ŷ˿ڣ�����FTPЭ�飬����������·
void login(){
    char buffer[MAXSIZE];
    struct sockaddr_in server_addr;
    int numbytes;
    
    if((host=gethostbyname(serverAddr))==NULL){
        printf("�޷����ʷ�����: %s\n",serverAddr);
		exit(1);
    }
    
    /* if((portnumber=atoi(argv[2]))<0)
    {
        fprintf(stderr,"Usage:%s hostname portnumber\a\n",argv[0]);
        exit(1);
    } */
    
    if((sockfd=socket(AF_INET,SOCK_STREAM,0))==-1){
        printf("socket() ʧ��\n");
		exit();
    }
    
    bzero(&server_addr,sizeof(server_addr));
    server_addr.sin_family=AF_INET;
    server_addr.sin_port=htons(SERV_COM_PORT);
    memcpy(&server.sin_addr,host->h_addr_list[0],host->h_length);
    
    if(connect(sockfd,(struct sockaddr*)(&server_addr),sizeof(struct sockaddr))==-1){
        printf("�������21�˿ڽ�������ʧ��");
        exit(1);
    }
    
    while(true){
        if((numbytes=read(sockfd,buffer,MAXSIZE)==-1){
            printf("read error1");
            exit(1);
        }
        buffer[numbytes]="\0";
        
        //���յ�FTP�����͵Ļ�ӭ��Ϣ�������û���
        if(strncmp(buffer,"220",3)==0){
            printf("%s\n",buffer);
            buffer="USER anonymous";
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("�����û���ʧ��\n");
                exit(1);
            }
            printf("%s\n",buffer);
        }
        //���յ�FTP���������͵� �û������ڲ����ǺϷ��û�����Ϣ����������
        else if(strncmp(buffer,"331",3)==0){
            printf("%s\n",buffer);
            buffer="PASS ttluze@gmail.com";
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("��������ʧ��\n");
                exit(1);
            }
        }
        //���յ����������͵� �û�����������ȷ����Ϣ
        else if(strncmp(buffer,"230",3)==0){
            printf("��¼FTP�������ɹ�\n");
            break;
        }
    }
}

//ѡ��FTP�Ĺ���ģʽ
//PORT,��Ҫ��������ʱ���ͻ���ͨ��21�˿ڷ���PORT��������а����ͻ��˽������ݵĶ˿�
//��������20�Ŷ˿���֮���ӣ��ͻ����յ�200ʱ��ʾ������·�������
//PASV����Ҫ��������ʱ���ͻ���ͨ��21�˿ڷ���PASV������յ������а���227�������������//·�˿ںţ��ӿͻ��˷�������
void ftp_mode(int newMode){
    workMode=newMode;
}

//ע���������Ĵ����Ƿ�Ҫexit(1)
int getDataSocket(){
    char buffer[MAXSIZE];
    int numbytes;
    int data_sockfd;
    
    switch(workMode){
        PORT_MODE:
            //client_addr������bind�׽��֣�local_addr��������ȡ����ip
            struct sockaddr_in client_addr,local_addr;
            int addr_len=sizeof(struct sockaddr_in);
            int ret;
            char local_ip[24];
            char buffer[MAXSIZE];
            int listenSockfd;
            int i=0;
            
            if((listenSockfd=socket(AF_INET,SOCK_STREAM,0))==-1){
                printf("PORT�����׽���ʧ��\n");
            }
            
            //PORT����͸��������ͻ��˵�ip�Ͷ˿�(������·)
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
                printf("�����ͻ���data_socketʧ��\n");
                exit(1);
            }
            //get local host's ip
            if(getsockname(sockfd,(struct sockaddr*)&localaddr,&ret) < 0){
                printf("��ȡ����IPʧ��\n");
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
                
            //����PORT����
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("����ָ��PORTʧ��\n");
                exit(1);
            }
            printf("%s",buffer);
                
            //���շ��������ص���Ϣ
            if((numbytes=read(sockfd,buffer,MAXSIZE)==-1){
                printf("read error getDataSocket");
                exit(1);
            }
            buffer[numbytes]="\0";
            
            if(strncmp(buffer,"200",3)==-1){
                printf("ָ��PORTû�з���200");
                exit(1);
            }
            printf("%s\n",buffer);

            //�ɹ����յ�PORT200���������Է���˵�����
            while(i < 3){
                //�����client_addr��ʾFTP��������addr
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
            //����PASV
            struct sockaddr_in serv_addr;
            buffer="PASV\r\n";
            if(write(sockfd,buffer,strlen(buffer))==-1){
                printf("����ָ��PASVʧ��\n");
                exit(1);
            }
            printf("%s",buffer);
            
            //���շ��������ص���Ϣ
            if((numbytes=read(sockfd,buffer,MAXSIZE)==-1){
                printf("read error getDataSocket");
                exit(1);
            }
            buffer[numbytes]="\0";
            
            if(strncmp(buffer,"227",3)==-1){
                printf("ָ��PASVû�з���227");
                exit(1);
            }
            printf("%s\n",buffer);
            
            //��ȡ����˵����ݶ˿ڣ���buffer�ж�ȡ(192,168,1,1,x,y)
            //�˿�Ϊx*256+y
            //strrchr����Ϊ����ַ����һ�γ��ֵ�λ�õ�ָ��
            int serverPort;
            char* buf_ptr;
            buf_ptr=strrchr(buffer,')');
            *buf_ptr='\0';
            buf_ptr = strrchr(buffer, ',');
            serverPort = atoi(buf_ptr + 1);
            *buf_ptr = '\0';
            buf_ptr = strrchr(buffer, ',');
            serverPort += atoi(buf_ptr + 1) * 256;
            
            //�����׽��֣�����������
            if((data_sockfd=socket(AF_INET,SOCK_STREAM,0))==-1){
                printf("�����׽���ʧ��,getdatasocket\n");
                exit(1);
            }
            bzero(&server_addr,sizeof(server_addr));
            server_addr.sin_family=AF_INET;
            serv_addr.sin_port=htons(serverPort);
            memcpy(&server.sin_addr,host->h_addr_list[0],host->h_length);
            
            if(connect(data_sockfd,(struct sockaddr*)(&server_addr),sizeof(struct sockaddr))==-1){
                printf("PASVģʽ����������·ʧ��\n");
                exit(1);
            }
            break;
        default:
            printf("FTP����ģʽֻ����PORT����PASV");
            exit(1);
    }
    return data_sockfd;
}

//��ȡ�ļ�������
//����ֵΪ-1ʱ��ʾ����ʧ��,����ֵΪ-2��ʾ�û�ȡ������
int getFile(char *local_file){
    int filefd = -1;
	struct stat* buf;
	char usr_input;
    
	if (0 == access(local_file, F_OK)){//�ж��ļ��Ƿ����
        //���ļ��Ѵ���
		buf = (struct stat*)malloc(sizeof(struct stat));
		stat(local_file, buf);//���ļ�״̬����buf
		if (S_ISDIR(buf->st_mode)){
			printf("%s ��һ��Ŀ¼\n", local_file);
			free(buf);
			return -1;
		}
        
        printf("%s�ļ��Ѵ��ڣ�ѡ�񸲸�(1)��ȡ������(2)����\n",local_path);
        usr_input=(char)getchar();
        if(user_input=='2'){
            return -2;
        }
        else{
            //ɾ��ԭ�ļ�
            filefd = open(local_file, O_TRUNC | O_WRONLY);
        }
        //����Ĵ�����FTP������֧�ֶϵ�����ʱ
        
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
        //���ļ�������
		filefd = open(local_file, O_CREAT | O_WRONLY, S_IRUSR|S_IWUSR | S_IRGRP | S_IROTH);
	}
	if (filefd==-1){
		return -1;
	}
    return filefd;
}

//�������ݣ���srcfd��destfd
int file_copy(int srcfd,int destfd,int *psize){
    char* buf;
	char* ptr;
	int read_size = 0;//�ӷ�����һ��read�����ĳ���
	int write_size = 0;//���ļ���һ��writeд�ĳ���
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

//��ȡ�������ļ��б�
//LIST
void ftp_list(){
    char buffer[MAXSIZE];
    int numbytes;
    int data_sockfd=getDataSocket();
    
    buffer="LIST/r/n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("����LISTָ��ʧ��\n");
        close(data_sockfd);
        return;
    }
    printf("%s",buffer);
    
    //���շ��񷵻ص���Ϣ
    if(numbytes=read(data_sockfd,buffer,MAXSIZE)==-1){
        printf("����LISTָ��󣬽��շ��񷵻ص���Ϣʧ��\n");
        close(data_sockfd);
        return;
    }
    buffer[numbytes]="\0";
    if(strncmp(buffer,"226",3)==-1){
        printf("ָ��LISTû�з���226\n");
        close(data_sockfd);
        return;
    }
    printf("%s\n",buffer);
    close(data_sockfd);
    return;
}

//Ŀ¼����
//CWD,���������Ӳ�����ֻͨ��sockfd��������
void ftp_cd(char *dir){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="CWD ";
    strcat(buffer,dir);
    strcat(buffer,"\r\n");
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("����CWDָ��ʧ��\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("����CWDָ��󣬽��շ�����������Ϣʧ��\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"250",3)==-1){
        printf("ָ��CWDû�з���250\n");
        return;
    }
    printf("%s\n",buffer);
    return;
}

//Ŀ¼���� CDUP
//������һ��
void ftp_back(){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="CDUP\r\n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("����CDUPָ��ʧ��\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("����CDUPָ��󣬽��շ�����������Ϣʧ��\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"250",3)==-1){
        printf("ָ��CDUPû�з���250\n");
        return;
    }
    printf("%s\n",buffer);
    return;
}

//���ص�ǰ����Ŀ¼
//PWD
void ftp_pwd(){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="PWD\r\n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("����PWDָ��ʧ��\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("����PWDָ��󣬽��շ�����������Ϣʧ��\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"257",3)==-1){
        printf("ָ��PWDû�з���250\n");
        return;
    }
    printf("%s\n",buffer);
    return;
}

//����Ŀ¼
//CMD
void ftp_mkdir(){
    
}

//�˳���¼
//QUIT
void ftp_quit(){
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="QUIT\r\n";
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("����QUITָ��ʧ��\n");
        return;
    }
    printf("%s",buffer);
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("����QUITָ��󣬽��շ�����������Ϣʧ��\n");
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"221",3)==-1){
        printf("ָ��QUITû�з���250\n");
        return;
    }
    printf("%s\n",buffer);
    close(sockfd);
    return;
}

//�ϴ�����
//sockfd��������RETR
//data_sockfd�����ļ�
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
        printf("����RETRָ��ʧ��\n");
        close(data_sockfd);
        return;
    }
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("����RETRָ��󣬽��շ�����������Ϣʧ��\n");
        close(data_sockfd);
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"400",3)==-1){
        printf("ָ��RETRû�з���400\n");
        close(data_sockfd);
        return;
    }
    //�����׽����Ѿ�����������Ѿ����ܵ����������صĳɹ���Ϣ�������������׽����л�ȡ����
    if(file_copy(data_sockfd,filefd,&numbytes)==-1){
        printf("����ʧ��\n");
        return;
    }
    close(filefd);
    close(data_sockfd);
    printf("���سɹ�����������Ϊ%d\n",numbytes);
    return;
}


//�ϴ�����
//sockfd��������RETR
//data_sockfd�����ļ�
void ftp_upload(char* local_file, char* remote_file){
    int filefd;
    int numbytes;
    
    if((filefd=open(local_file, O_RDONLY))==-1)
	{
		printf("%s�ļ���ʧ��\n",local_file);
		return;
	}
    
    int data_sockfd=getDataSocket();
    char buffer[MAXSIZE];
    int numbytes;
    
    buffer="STOR ";
    strcat(buffer,remote_file);
    strcat(buffer,"/r/n");
    if(write(sockfd,buffer,strlen(buffer))==-1){
        printf("����STORָ��ʧ��\n");
        close(data_sockfd);
        return;
    }
    
    if((numbytes=read(sockfd,buffer,MAXSIZE))==-1){
        printf("����STORָ��󣬽��շ�����������Ϣʧ��\n");
        close(data_sockfd);
        return;
    }
    buffer[numbytes]='\0';
    if(strncmp(buffer,"150",3)==-1){
        printf("ָ��STORû�з���150\n");
        close(data_sockfd);
        return;
    }
    //�����׽����Ѿ�����������Ѿ����ܵ����������صĳɹ���Ϣ�������������׽����л�ȡ����
    if(file_copy(filefd,data_sockfd,&numbytes)==-1){
        printf("�ϴ�ʧ��\n");
        return;
    }
    close(filefd);
    close(data_sockfd);
    printf("�ϴ��ɹ����ϴ�����Ϊ%d\n",numbytes);
    return;
}

//�����ȡ����һ���˿�
int random_local_port(){
	int local_port;
	srand((unsigned)time(NULL));
	local_port = rand() % 40000 + 1025;
	return local_port;
}

