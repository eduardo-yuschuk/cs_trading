#include <iostream>
#include <random>
#include <functional>
#include <cassert>
#include <chrono>
#include "ta_libc.h"
#include <cstddef>
#include <cstdio>
#include <cstdlib>
#include <cstring>
using namespace std;

namespace text2bin {
	
	typedef struct {
		float *values;
		int size;
		int count;
	} quotes_t;

	void init_quotes(quotes_t *quotes) {
		memset(quotes, 0, sizeof(quotes_t));
	}

	static FILE * open_prices_file(char *ticks_path) {
		FILE *fd = fopen(ticks_path, "r");
		if (fd == NULL) {
			char buffer[512];
			sprintf(buffer, "fopen: %s", ticks_path);
			perror(buffer);
			exit(1);
		}
		return fd;
	}

	static void grow_quotes(quotes_t *quotes) {
		quotes->size += 1000000;
		quotes->values = (float *)realloc(quotes->values, quotes->size * sizeof(float));
		if (quotes->values == NULL) {
			perror("realloc");
			exit(1);
		}
	}

	static void reserve_quotes(quotes_t *quotes, int count) {
		quotes->size = count;
		quotes->values = (float *)realloc(quotes->values, quotes->size * sizeof(float));
		if (quotes->values == NULL) {
			perror("realloc");
			exit(1);
		}
	}

	static void add_price(quotes_t *quotes, double price) {
		if (quotes->count == quotes->size) {
			grow_quotes(quotes);
			printf("quotes growth to %d elements\n", quotes->size);
		}
		quotes->values[quotes->count] = price;
		quotes->count++;
	}

	void load_prices(char *ticks_path, quotes_t *quotes) {
		FILE *fd = open_prices_file(ticks_path);
		float price;
		/* skip the first line */
		fscanf(fd, "%*[^\n]");
		int captured = 1;
		while (captured == 1) {
			captured = fscanf(fd, "%*[^,],%f,%*[^\n]", &price);
			add_price(quotes, price);
		}
		fclose(fd);
	}

	static FILE * open_quotes_file(char *quotes_path, char *mode) {
		FILE *fd = fopen(quotes_path, mode);
		if (fd == NULL) {
			char buffer[512];
			sprintf(buffer, "fopen: %s", quotes_path);
			perror(buffer);
			exit(1);
		}
		return fd;
	}

	void save_quotes(char *quotes_path, quotes_t *quotes) {
		FILE *fd = open_quotes_file(quotes_path, "w+b");
		float *values = quotes->values;
		int remaining = quotes->count;
		int written = 0;
		while (remaining > 0) {
			written = fwrite(values, sizeof(float), remaining, fd);
			printf("%d/%d prices saved\n", written, quotes->count);
			remaining -= written;
			values += written;
		}
		fclose(fd);
	}

	static size_t get_file_size(FILE *fd) {
		fseek(fd, 0, SEEK_END);
		size_t size = ftell(fd);
		rewind(fd);
		return size;
	}

	void load_quotes(char *quotes_path, quotes_t *quotes) {
		FILE *fd = open_quotes_file(quotes_path, "r+b");
		size_t file_size = get_file_size(fd);
		int prices_count = file_size / sizeof(float);
		reserve_quotes(quotes, prices_count);
		float *values = quotes->values;
		int remaining = prices_count;
		size_t readed;
		while (remaining > 0) {
			readed = fread(values, 1, remaining, fd);
			printf("%d/%d prices readed\n", readed, quotes->count);
			remaining -= readed;
			values += readed;
		}
		quotes->count = prices_count;
		fclose(fd);
	}

	void initial_load(char *ticks_path, char *quotes_path) {
		quotes_t quotes;
		init_quotes(&quotes);
		load_prices(ticks_path, &quotes);
		printf("%d quotes readed\n", quotes.count);
		printf("first: %.5f\n", quotes.values[0]);
		printf("last: %.5f\n", quotes.values[quotes.count - 1]);
		save_quotes(quotes_path, &quotes);
	}

	int file_exist(char *filename) {
		struct stat buffer;
		return (stat(filename, &buffer) == 0);
	}

	void import_text_data() {
		char ticks_path[] = "E:\\EURUSD_UTC_Ticks_Bid_2016.01.01_2016.07.02.csv";
		char quotes_path[] = "E:\\PRICES.bin";
		if (!file_exist(quotes_path)) {
			initial_load(ticks_path, quotes_path);
		}
		quotes_t quotes;
		init_quotes(&quotes);
		load_quotes(quotes_path, &quotes);
		printf("%d quotes loaded\n", quotes.count);
	}
}

namespace random_series {
	constexpr int pricesCount = 100;
	float base_serie[pricesCount];

	void generate() {
		default_random_engine generator;
		uniform_int_distribution<int> distribution(112000, 112100);
		auto dice = bind(distribution, generator);
		
		for (int i = 0; i < pricesCount; i++) {
			base_serie[i] = dice() / 100000.0f;
			//cout << prices[i] << endl;
		}
	}
}

namespace indicators_compute {

	void generate_indicators() {
		for (int i = 0; i < 5000000; i++)
		{
			int startIdx = 0;
			int endIdx = random_series::pricesCount - 1;
			int optInTimePeriod = 15; // From 2 to 100000
			int outBegIdx;
			int outNBElement;
			double outReal[100];
			TA_RetCode ret = TA_S_RSI(startIdx, endIdx, random_series::base_serie, optInTimePeriod, &outBegIdx, &outNBElement, outReal);
			assert(ret == 0);
		}
		//cout << "outBegIdx: " << outBegIdx << endl;
		//cout << "outNBElement: " << outNBElement << endl;
		//cout << "ret: " << ret << endl;
	}
}

int main()
{
	auto start = chrono::system_clock::now();
	// your code starts here
	text2bin::import_text_data();
	// your code ends here
	auto end = chrono::system_clock::now();
	auto duration = end - start;
	cout << "Time: " << chrono::duration_cast<chrono::milliseconds>(duration).count() / 1000.f << endl;
	system("pause");
	return 0;
}

