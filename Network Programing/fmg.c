#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <unistd.h>
#include <arpa/inet.h>
#include <sys/types.h>
#include <sys/socket.h>

#define STR_SIZE 10
#define BUF_SIZE 100

int sock;
struct sockaddr_in serv_addr;
char buf[BUF_SIZE];
int str_len;

int nMoney = 0;

struct Item
{
	char name[STR_SIZE];
	int Count;
};

char str1[STR_SIZE], str2[STR_SIZE], str3[STR_SIZE];
struct Item ItemAry[3] = {0, };
int nMoney;

void InitialSet()
{
	srand(time(0));

	for (int i = 0; i < 3; i++)
		memset(&ItemAry[i].name, 0, STR_SIZE);

	strcat(ItemAry[0].name, "Branch");
	strcat(ItemAry[1].name, "Mineral");
	strcat(ItemAry[2].name, "Meat");

	memset(&str1, 0, STR_SIZE);
	memset(&str2, 0, STR_SIZE);
	memset(&str3, 0, STR_SIZE);

	strcat(str1, "Adventure");
	strcat(str2, "Sale");
	strcat(str3, "MarketPrice");
}

int RandomNumber;
int RandomCount;
void Adventure()
{
	RandomNumber = (rand() % 3) + 0;
	RandomCount = (rand() % 5 + 1);
	ItemAry[RandomNumber].Count += RandomCount;
	printf("+%s: %d\n", ItemAry[RandomNumber].name, RandomCount);
}

void Sale(char s1[], int n)
{	
	int Count;
	
	for (int i = 0; i < 3; i++)
	{
		if (strcmp(ItemAry[i].name, s1) == 0)
		{
			if (ItemAry[i].Count >= n)
			{
				Count = n;	
				ItemAry[i].Count -= n;
			}
			else
			{
				Count = ItemAry[i].Count;
				ItemAry[i].Count = 0;
			}
			
			printf("-%s: %d\n", ItemAry[i].name, Count);
			
			if (Count > 0)
			{
				memset(&buf, 0, BUF_SIZE);
				strcat(buf, s1);
				strcat(buf, ",");
				char itoac[BUF_SIZE];
				sprintf(itoac, "%d", Count);
				strcat(buf, itoac);

				write(sock, buf, strlen(buf));
				read(sock, buf, BUF_SIZE-1);
				int co = atoi(buf);
				printf("+Money: %d\n", co);
				nMoney += co;
			}
			
			break;
		}
	}
}

void ShowMarketPrice()
{
	memset(buf, 0, BUF_SIZE);
	strcat(buf, "Price");
	write(sock, buf, strlen(buf));
	memset(buf, 0, BUF_SIZE);
	read(sock, buf, BUF_SIZE-1);
	printf("%s\n", buf);
}

void Print()
{
	printf("Holding amount:[Money: %d][%s: %d][%s: %d][%s: %d]\n", nMoney,
		ItemAry[0].name, ItemAry[0].Count,
		ItemAry[1].name, ItemAry[1].Count,
		ItemAry[2].name, ItemAry[2].Count);
}

void Play()
{
	while (1)
	{
		fputs("Input select[Q to quit][Adventure][Sale][MarketPrice] : ", stdout);
		char Inputstr1[STR_SIZE];
		char Inputstr2[STR_SIZE];
		int Count;
		
		scanf("%s", Inputstr1);
		
		if(!strcmp(buf, "q\n"))
			break;

		if (strcmp(Inputstr1, "Q") == 0 || strcmp(Inputstr1, "q") == 0)
			break;
		if (strcmp(str1, Inputstr1) == 0)
		{
			Adventure();
			Print();
		}
		else if (strcmp(str2, Inputstr1) == 0)
		{
			scanf("%s", Inputstr2);
			scanf("%d", &Count);
			Sale(Inputstr2, Count);
			Print();
		}
		else if (strcmp(str3, Inputstr1) == 0)
		{
			ShowMarketPrice();
			Print();
		}
		else
			printf("\n");
		
		printf("--------------------------------------------------------------------\n");
	}
}

void error_handling(char *buf)
{
	fputs(buf, stderr);
	fputc('\n', stderr);
	exit(1);
}

int main(int argc, char* argv[])
{
	if(argc != 3) 
	{
		printf("Usage : %s <IP> <port> \n", argv[0]);
		exit(1);
	}

	sock = socket(PF_INET, SOCK_STREAM, 0);
	if(sock == -1)
		error_handling("socket() error");

	memset(&serv_addr, 0, sizeof(serv_addr));
	serv_addr.sin_family = AF_INET;
	serv_addr.sin_addr.s_addr = inet_addr(argv[1]);
	serv_addr.sin_port = htons(atoi(argv[2]));

	if(connect(sock, (struct sockaddr*)&serv_addr, sizeof(serv_addr)) == -1)
		error_handling("connect() error");
	else
		fputs("Connected. . . . . . . . . .\n", stdout);
	
	InitialSet();
	
	Play();
	
	close(sock);

	return 0;
}