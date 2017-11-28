#ifndef RESOURCE_H
#define RESOURCE_H

#endif // RESOURCE_H

/* 定义域名查询和 socket 消息 */
#define CTR_TRY_TIMES   3   //命令套接字失败重试次数
#define CTR_TRY_INTERVAL  2000 //命令套接字失败重试间隔
#define RECV_BUF_LEN 1024 //接收命令字符数组长度

/* 定义 FTP 的状态 */
#define FTPS_CLOSED    0
#define FTPS_GETHOST   1
#define FTPS_CONNECT   2
#define FTPS_USER      3
#define FTPS_PASSWD    4
#define FTPS_LOGIN     5
#define FTPS_SENDCMD   6
/* FTP 错误码 */
#define FTPE_SUCESS    0  /* 成功 */
#define FTPE_ERROR    -1  /* 一般错误 */
#define FTPE_NOMEM    -2  /* 没有内存 */
#define FTPE_INVALID  -3  /* 无效参数 */
#define FTPE_STATUS   -4  /* 状态错误 */
#define INVALID_HANDLE_VALUE ((HANDLE)(LONG_PTR)-1)
