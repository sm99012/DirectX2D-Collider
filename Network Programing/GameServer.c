#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <arpa/inet.h>
#include <sys/socket.h>
#include <sys/epoll.h>

#define BUF_SIZE 100
#define EPOLL_SIZE 50

void error_handling(char *message)
{
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}

int nMoneyAry[3] = { 10, 10, 10 };
int nCountAry[3] = { 10, 10, 10 };
int nMoney = 0;

char transportbuf[BUF_SIZE];
char temp1[20];
char temp2[20];
char temp3[20];

int Carculate(char str[], int count);

int main(int argc, char* argv[])
{	
	int serv_sock, clnt_sock;
	struct sockaddr_in serv_adr, clnt_adr;
	socklen_t adr_sz;
	int str_len, i;
	char buf[BUF_SIZE];
	
	struct epoll_event *ep_events;
	struct epoll_event event;
	int epfd, event_cnt;
	
	if (argc != 2)
	{
		printf("Usage : %s <port>\n", argv[0]);
		exit(1);
	}
	
	serv_sock = socket(PF_INET, SOCK_STREAM, 0);
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(atoi(argv[1]));
	
	if (bind(serv_sock, (struct sockaddr*)&serv_adr, sizeof(serv_adr)) == -1)
		error_handling("bind() error");
	
	if (listen(serv_sock, 5) == -1)
		error_handling("listen() error");
	
	epfd = epoll_create(EPOLL_SIZE);
	ep_events = malloc(sizeof(struct epoll_event)*EPOLL_SIZE);
	
	event.events = EPOLLIN;
	event.data.fd = serv_sock;
	epoll_ctl(epfd, EPOLL_CTL_ADD, serv_sock, &event);

	while (1)
	{
		event_cnt = epoll_wait(epfd, ep_events, EPOLL_SIZE, -1);
		if (event_cnt == -1)
		{
			puts("epoll_wait() error");
			break;
		}
		
		for (i = 0; i < event_cnt; i++)
		{
			if (ep_events[i].data.fd == serv_sock)
			{
				adr_sz = sizeof(clnt_adr);
				clnt_sock = accept(serv_sock, (struct sockaddr*)&clnt_adr, &adr_sz);
				event.events = EPOLLIN;
				event.data.fd = clnt_sock;
				epoll_ctl(epfd, EPOLL_CTL_ADD, clnt_sock, &event);
				//printf("connected Player: %d\n", clnt_sock);
			}
			else
			{
				str_len = read(ep_events[i].data.fd, buf, BUF_SIZE);
				if (str_len == 0)
				{
					epoll_ctl(epfd, EPOLL_CTL_DEL, ep_events[i].data.fd, NULL);
					close(ep_events[i].data.fd);
					//printf("closed Player: %d\n", ep_events[i].data.fd);
				}
				else
				{
					if (strcmp(buf, "Price") == 0)
					{
						memset(&transportbuf, 0, BUF_SIZE);
						memset(&temp1, 0, BUF_SIZE);
						memset(&temp2, 0, BUF_SIZE);
						memset(&temp3, 0, BUF_SIZE);
						
						strcat(transportbuf, "Market Price:[");
						strcat(transportbuf, "Branch: ");
						sprintf(temp1, "%d", nMoneyAry[0]);
						strcat(transportbuf, temp1);
						strcat(transportbuf, "][Mineral: ");
						sprintf(temp2, "%d", nMoneyAry[1]);
						strcat(transportbuf, temp2);
						strcat(transportbuf, "][Meat: ");
						sprintf(temp3, "%d", nMoneyAry[2]);
						strcat(transportbuf, temp3);
						strcat(transportbuf, "]");
						
						printf("%s\n", transportbuf);
						write(ep_events[i].data.fd, transportbuf, strlen(transportbuf));
					}
					else
					{
						char* token1;
						char* token2;
						token1 = strtok_r(buf, ",", &token2);
						int co = atoi(token2);
						//printf("%s, ", token1);
						//printf("%d\n", co);

						nMoney = Carculate(token1, co);

						memset(&buf, 0, BUF_SIZE);
						sprintf(buf, "%d", nMoney);

						write(ep_events[i].data.fd, buf, strlen(buf));
					}
				}
			}
		}
	}
	close(serv_sock);
	close(epfd);
	
	return 0;
}

int Carculate(char str[], int count)
{
	int money;
	int AryNum;
	
	if (strcmp(str, "Branch") == 0)
		AryNum = 0;
	else if (strcmp(str, "Mineral") == 0)
		AryNum = 1;
	else if (strcmp(str, "Meat") == 0)
		AryNum = 2;
	
	money = nMoneyAry[AryNum] * count;
	nCountAry[AryNum] += count;
	
	float rateofchange = ((float)nCountAry[AryNum]/(float)(nCountAry[AryNum] + count));
	
	for (int j = 0; j < 3; j++)
	{
		if (j == AryNum)
			nMoneyAry[j] = (int)((float)nMoneyAry[j] * rateofchange);
		else
			nMoneyAry[j] += (int)((float)nMoneyAry[j] * (((float)1 - rateofchange) / 2));
	}
	
	memset(&transportbuf, 0, BUF_SIZE);
	memset(&temp1, 0, BUF_SIZE);
	memset(&temp2, 0, BUF_SIZE);
	memset(&temp3, 0, BUF_SIZE);

	strcat(transportbuf, "Market Price:[");
	strcat(transportbuf, "Branch: ");
	sprintf(temp1, "%d", nMoneyAry[0]);
	strcat(transportbuf, temp1);
	strcat(transportbuf, "][Mineral: ");
	sprintf(temp2, "%d", nMoneyAry[1]);
	strcat(transportbuf, temp2);
	strcat(transportbuf, "][Meat: ");
	sprintf(temp3, "%d", nMoneyAry[2]);
	strcat(transportbuf, temp3);
	strcat(transportbuf, "]");

	printf("%s\n", transportbuf);
	
	return money;
}