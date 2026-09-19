using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class matrixShenanigans
{

	static void log(int ModuleId, string message) {Debug.Log(message.Split('\n').Select(x => $"[Cruel Ship Review #{ModuleId}] {x}").Aggregate((a,b) => a+ "\n" + b));}
	static string listToString(List<int> list) => $"[{string.Join(" ", list)}]";
	static string listToString(List<string> list) => $"[{string.Join(" ", list)}]";
	static string listToString(List<double> list) => $"[{string.Join(" ", list.Select(x => x.ToString("F4")))}]";
	static string matrixToString(List<List<int>> list) => listToString(list.Select(listToString).ToList());
	static string matrixToString(List<List<double>> list) => listToString(list.Select(listToString).ToList());
	
	public static bool initialCheck(List<int> data)
	{
		if (det(getProperMatrixFromData(data)) == 0) return false;
		List<List<int>> A = getProperMatrixFromData(data);
		List<List<int>> B = multiplyMatrices(t(A),A);
		List<double> lambdas = SolveCubic(
			-1,
			B[0][0]+B[1][1]+B[2][2],
			-Enumerable.Range(0,3).Select(
				i => det(
					B.Select(
						x => x.Where(
							(_0,j) => j != i
						).ToList()
					).Where((_0, j) => j != i).ToList()
				)).Sum(),
			det(B)
		).ConvertAll(x => x.Re).OrderByDescending(x=>x).ToList();
		foreach (var lambda in lambdas)
		{
			try
			{
				List<List<double>> Bl = B.Select((x, i) => x.Select((y, j) => i == j ? y - lambda : y).ToList())
					.ToList();
				double k1 = Bl[1][0] / Bl[0][0], k2 = Bl[2][0] / Bl[0][0];
				Bl[1] = Bl[1].Select((x, i) => x - k1 * Bl[0][i]).ToList();
				Bl[2] = Bl[2].Select((x, i) => x - k2 * Bl[0][i]).ToList();
				double k3 = Bl[2][1] / Bl[1][1];
				Bl[2] = Bl[2].Select((x, i) => x - k3 * Bl[1][i]).ToList();
				if (Math.Abs(Bl[2][2]) > 0.000001) return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
		return true;
	}
	public static List<double> getFlattenedSVDFromData(List<int> data, int ModuleId)
	{
		var svd = SVD(getProperMatrixFromData(data), ModuleId);
		log(ModuleId, $"Matrix R: {matrixToString(svd)}.");
		return Enumerable.Range(0, 9).Select(i => svd[i / 3][i % 3]).ToList();
	}
	
	
	
	

	private static List<List<int>> multiplyMatrices(List<List<int>> A, List<List<int>> B)=>
		Enumerable.Range(0, A.Count).Select(r => 
			Enumerable.Range(0, B[0].Count).Select(c => Enumerable.Range(0, B.Count).Select(i => A[r][i] * B[i][c]).Sum()
				).ToList()
		).ToList();
	private static List<List<double>> multiplyMatrices(List<List<int>> A, List<List<double>> B)=>
		Enumerable.Range(0, A.Count).Select(r => 
			Enumerable.Range(0, B[0].Count).Select(c => Enumerable.Range(0, B.Count).Select(i => A[r][i] * B[i][c]).Sum()
			).ToList()
		).ToList();
	private static List<List<double>> multiplyMatrices(List<List<double>> A, List<List<double>> B)=>
		Enumerable.Range(0, A.Count).Select(r => 
			Enumerable.Range(0, B[0].Count).Select(c => Enumerable.Range(0, B.Count).Select(i => A[r][i] * B[i][c]).Sum()
			).ToList()
		).ToList();
	
	private static List<List<double>> multiplyMatrixAndVector(List<List<int>> A, List<double> v) => multiplyMatrices(A, Enumerable.Range(0, A.Count).Select(i=>new List<double>{v[i]}).ToList());
	
	// Converts flattened 6x6 to proper 3x3 
	private static List<List<int>> getProperMatrixFromData(List<int> data)
	{
		return Enumerable.Range(0,4).Select(i => Enumerable.Range(0,3).Select(x =>
						Enumerable.Range(0,3).Select(y => data[i%2*3 + i/2*18 + 6*x + y]
							).ToList()).ToList()).Aggregate(multiplyMatrices);
	}

	private static List<List<T>> t<T>(List<List<T>> A) => Enumerable.Range(0,A.Count).Select(i=>Enumerable.Range(0,A[0].Count).Select(j=>A[j][i]).ToList()).ToList();

	private static long det(List<List<int>> A) => A.Count == 1 ? A[0][0] :
		A[0].Select((_0, i) => 
			(i % 2 == 0 ? 1 : -1) * A[0][i] *
			det(A.Skip(1).Select(row => row.Where((_1, j) => j != i).ToList()).ToList())
		).Sum();
	
	// Does the SVD for 3x3 (specifically 3x3)
	private static List<List<double>> SVD(List<List<int>> A, int ModuleId)
	{
		log(ModuleId, $"Matrix A: {matrixToString(A)}.");
		List<List<int>> B = multiplyMatrices(t(A),A);
		log(ModuleId, $"Matrix B: {matrixToString(B)}.");
		List<double> lambdas = SolveCubic(
				-1,
				B[0][0]+B[1][1]+B[2][2],
				-Enumerable.Range(0,3).Select(
					i => det(
						B.Select(
							x => x.Where(
								(_0,j) => j != i
								).ToList()
							).Where((_0, j) => j != i).ToList()
						)).Sum(),
				det(B)
			).ConvertAll(x => x.Re).OrderByDescending(x=>x).ToList();
		log(ModuleId, $"Lambdas: {listToString(lambdas)}.");
		List<double> sigmas = lambdas.Select(Math.Sqrt).ToList();
		log(ModuleId, $"Sigmas: {listToString(sigmas)}.");
		List<List<double>> V = Enumerable.Range(0,3).Select(_ => new List<double>{0,0,0}).ToList();
		List<List<double>> U = Enumerable.Range(0,3).Select(_ => new List<double>{0,0,0}).ToList();
		for (int z=0; z < lambdas.Count; z++)
		{
			List<List<double>> Bl = B.Select((x, i) => x.Select((y, j) => i == j ? y - lambdas[z] : y).ToList())
				.ToList();
			log(ModuleId, $"Matrix B-lambda{z+1}: {matrixToString(Bl)}.");
			//Bl * V = 0.
			double k1 = Bl[1][0] / Bl[0][0], k2 = Bl[2][0] / Bl[0][0];
			Bl[1] = Bl[1].Select((x, i) => x - k1 * Bl[0][i]).ToList();
			Bl[2] = Bl[2].Select((x, i) => x - k2 * Bl[0][i]).ToList();
			double k3 = Bl[2][1] / Bl[1][1];
			Bl[2] = Bl[2].Select((x, i) => x - k3 * Bl[1][i]).ToList();
			double K = Bl[0][1]/Bl[1][1];
			Bl[0] = Bl[0].Select((x,i)=> x - K * Bl[1][i]).ToList();
			double a = Bl[0][0], b = Bl[0][2], c = Bl[1][1], d = Bl[1][2];
			//meaning aV1 + bV3 = 0 ; cV2 + dV3 = 0. Take V3 = 1.
			// aV1 = -b => V1 = -b/a; hence V2 = -d/c.
			List<double> v = new List<double>{-b / a, -d / c, 1};
			log(ModuleId, $"Unnormed v{z+1}: {listToString(v)}.");
			v = norm(v);
			log(ModuleId, $"Normed v{z+1}: {listToString(v)}.");
			List<List<double>> sU0 = multiplyMatrixAndVector(A, v);
			List<double> u = sU0.Select(row => row[0]/sigmas[z]).ToList();
			log(ModuleId, $"Normed u{z+1}: {listToString(u)}.");

			for (int zz = 0; zz < 3; zz++)
			{
				V[zz][z] = v[zz];
				U[zz][z] = u[zz];
			}
		}
		log(ModuleId, $"Matrix U: {matrixToString(U)}.");
		log(ModuleId, $"Matrix V: {matrixToString(V)}.");
		return multiplyMatrices(U, t(V));
	}
	
	private static List<double> norm(List<double> v) => v.Select(y => y/Math.Sqrt(v.Select(x => x * x).Sum())).ToList();
	
	//took that from https://www.daniweb.com/programming/software-development/code/454493/solving-the-cubic-equation-using-the-complex-struct
	private static List<Complex> SolveCubic(double a, double b, double c, double d)
	{
		const int NRoots = 3;

		double SquareRootof3 = Math.Sqrt(3);
		// the 3 cubic roots of 1
		List<Complex> CubicUnity = new List<Complex>(NRoots) 
			{ new Complex(1, 0), new Complex(-0.5, -SquareRootof3 / 2.0), new Complex(-0.5, SquareRootof3 / 2.0) };
		// intermediate calculations
		double DELTA = 18 * a * b * c * d - 4 * b * b * b * d + b * b * c * c - 4 * a * c * c * c - 27 * a * a * d * d;
		double DELTA0 = b * b - 3 * a * c;
		double DELTA1 = 2 * b * b * b - 9 * a * b * c + 27 * a * a * d;
		Complex DELTA2 = new Complex(-27) * a * a * DELTA;
		Complex C = Complex.Pow((Complex.Pow(DELTA2, 0.5) + DELTA1) / 2, 1 / 3.0); //Phew...

		List<Complex> R = new List<Complex>(NRoots);
		for (int i = 0; i < NRoots; i++)
		{
			Complex M = CubicUnity[i] * C;
			Complex Root =  (M + b + DELTA0 / M) / (-3 * a);
			R.Add(Root);
		}
		return R;
	}
}

public struct Complex
{
    public double Re;
    public double Im;

    public Complex(double re, double im)
    {
        Re = re;
        Im = im;
    }
    public Complex(double re)
    {
	    Re = re;
	    Im = 0;
    }

    public double Magnitude => Math.Sqrt(Re * Re + Im * Im);
    public double Phase => Math.Atan2(Im, Re);

    public static Complex FromPolar(double magnitude, double phase)
    {
        return new Complex(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
    }

    public static Complex operator +(Complex a, Complex b) => new Complex(a.Re + b.Re, a.Im + b.Im);
    public static Complex operator +(Complex a, double b) => new Complex(a.Re + b, a.Im);
    public static Complex operator -(Complex a, Complex b) => new Complex(a.Re - b.Re, a.Im - b.Im);
    public static Complex operator -(Complex a) => new Complex(-a.Re, -a.Im);
    

    public static Complex operator *(Complex a, Complex b) =>
        new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);

    public static Complex operator *(Complex a, double s) => new Complex(a.Re * s, a.Im * s);

    public static Complex operator /(Complex a, Complex b)
    {
        double denom = b.Re * b.Re + b.Im * b.Im;
        return new Complex(
            (a.Re * b.Re + a.Im * b.Im) / denom,
            (a.Im * b.Re - a.Re * b.Im) / denom
        );
    }
    public static Complex operator /(double a, Complex b)
    {
	    double denom = b.Re * b.Re + b.Im * b.Im;
	    return new Complex(
		    (a * b.Re) / denom,
		    (-a * b.Im) / denom
	    );
    }
    public static Complex operator /(Complex a, double b) => new Complex(a.Re / b, a.Im / b);

    public static Complex Pow(Complex value, double power)
    {
        if (value.Re == 0 && value.Im == 0)
            return power == 0 ? new Complex(1, 0) : new Complex(0, 0);

        double newMagnitude = Math.Pow(value.Magnitude, power);
        double newPhase = value.Phase * power;
        return FromPolar(newMagnitude, newPhase);
    }

    public override string ToString() => $"{Re} + {Im}i";
}
