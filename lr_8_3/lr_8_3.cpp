#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>

#define ROOT 0

//Параллельная пузырьковая сортировка методом чет-нечетной перестановки

#define n 15000 
// Размерность массива

int *mas; double time0, time1;
int myrank, p;

void MStart() {
	int i;
	mas = (int*)malloc(n * sizeof(int));
	srand(1); //Инициализация псевдослучайного ряда
	printf("Amount of processes: %d\nOld Unsorted Array:\n", p);
	for (i = 0; i<n; i++) mas[i] = rand() % 15000;
	for (i = 0; i<n; i++) printf(i == 3 ? i = n - 4, "... " : "M[%d]=%d ", i, mas[i]);
	time0 = MPI_Wtime();
}

void MFinish() {
	int i;
	time1 = MPI_Wtime();
	printf("\nNew Sorted array:\n");
	for (i = 0; i<n; i++) printf(i == 3 ? i = n - 4, "... " : "M[%d]=%d ", i, mas[i]);
	printf("\nElapsed time: %lf\n", time1 - time0);
	free(mas);
}

int main(int argc, char **argv) {
	int i, j, k, prev, next;
	MPI_Status Status;
	MPI_Init(&argc, &argv);
	MPI_Comm_size(MPI_COMM_WORLD, &p);
	MPI_Comm_rank(MPI_COMM_WORLD, &myrank);
	if (myrank == ROOT) MStart();

	//  сортровка                         
	int bsize = n / p; //размер блока для каждого процесса
	int* buf = (int*)malloc((bsize + 1) * sizeof(int));
	//рассылка кусков масcива всем процессам
	MPI_Scatter(mas, bsize, MPI_INT, buf, bsize, MPI_INT, ROOT, MPI_COMM_WORLD);
	next = myrank + 1;
	prev = myrank - 1;
	/**/
	for (j = 0; j <= n - 1; j++)
	{
		//Четный проход
		for (i = 0; i <= bsize - 2; i += 2)
			if (buf[i]>buf[i + 1]) {
				k = buf[i];
				buf[i] = buf[i + 1];
				buf[i + 1] = k;
			}
		if (next<p) MPI_Send(&buf[bsize], 1, MPI_INT, next, 1, MPI_COMM_WORLD);
		if (prev >= 0) MPI_Recv(&buf[0], 1, MPI_INT, prev, 1, MPI_COMM_WORLD, &Status);

		//Нечетный проход    
		for (i = 1; i <= bsize - 1; i += 2)
			if (buf[i]>buf[i + 1]) {
				k = buf[i];
				buf[i] = buf[i + 1];
				buf[i + 1] = k;
			}
		if (next<p) MPI_Send(&buf[bsize], 1, MPI_INT, next, 1, MPI_COMM_WORLD);
		if (prev >= 0) MPI_Recv(&buf[0], 1, MPI_INT, prev, 1, MPI_COMM_WORLD, &Status);
	}
	
	//сборка массива из кусков
	MPI_Gather(buf, bsize, MPI_INT, mas, bsize, MPI_INT, ROOT, MPI_COMM_WORLD);

	free(buf);
	if (myrank == ROOT) MFinish();
	MPI_Finalize();

	return 0;
}
