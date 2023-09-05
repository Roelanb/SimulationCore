using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using NumSharp;
using MatplotlibCS.PlotItems;
using Deedle;

// create a 5,5,5,5 tensor of all ones
var t = tf.ones(new int[] { 5, 5, 5, 5 });

t = tf.reshape(t, new int[] { -1 });

Console.WriteLine(t);

var layers = keras.layers;

// load training data from https://storage.googleapis.com/tf-datasets/titanic/train.csv

// load csv file into memory



var train_data = Frame.ReadCsv("../../../data/train.csv");
var eval_data = Frame.ReadCsv("../../../data/eval.csv");

Console.WriteLine(train_data);

var y_Train = train_data.GetColumn<bool>("survived");
var y_Eval = eval_data.GetColumn<bool>("survived");

Console.WriteLine(y_Train);



//var y_Eval = eval_data.pop("survived");

//var h = train_data.head();


//foreach (var col in h.columns)
//{
//    Console.WriteLine($"{col.Name} {col.DType}");
//}

//// display the shape of the training data
//var d = train_data.describe();

//foreach (var item in y_Train.data)
//{
//    Console.WriteLine(item);
//}


