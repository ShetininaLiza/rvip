package rvip_lec;

public class Main {

	//количество процессов
	public static int N=3;
	//количество проходов по циклу для одного процесса
	public static int count = 10;
	//Массив, содержащий логические значения для каждого потока, 
	//поток i хочет попасть в критическую область или нет
	public static boolean[] choosing = new boolean[N]; 
	//Билет используется для определения приоритета
	public static int[] ticket = new int[N];
			
	public static void main(String[] args) {
		System.out.println("Алгоритм Лэмпорта");
		//Массив самих процессов
		MyThread[] threads=new MyThread[N];
		//Инициализация всех массивов
		InitMas(choosing, ticket, threads);
		//Запуск процессов
		for(int i=0; i<threads.length; i++)
		{
			threads[i].start();
		}
		//Ждем пока все потоки не закончат работу
		for (int i = 0; i < threads.length; i++) {
			try {
				threads[i].join();
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}
	private static void InitMas(boolean[] choosing, int[] ticket, MyThread[] threads)
	{
		for(int i=0; i<threads.length; i++)
		{
			threads[i]=new MyThread(i);
			choosing[i] = false;
			ticket[i] = 0;
		}
	}
}
