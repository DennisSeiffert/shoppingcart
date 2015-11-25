using NUnit.Framework;
using System;
using Caffe;
using Should;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace CaffeTests
{
	[TestFixture ()]
	public class CaffeTest
	{
		string netDefinition = @"
					name: 'TestNetwork'
	state {
	  phase: TEST 
	}
	input: 'data'
	input_shape { 
	  dim: 10 
	  dim: 3 
	  dim: 32 
	  dim: 32 
	} 
			layer { 
			  name: 'data'  
			  type: 'ImageData'  
			  top: 'data'  
			  top: 'label'  
			  include {  
			    phase: TRAIN  
			  }  
			  transform_param {  
			    scale: 0.00392156862745  
			    mirror: true  
			    crop_size: 32  
			  }  
			  image_data_param {  
			    source: '/home/dennis/PycharmProjects/untitled/images.txt'  
			    batch_size: 20  
			    new_height: 32  
			    new_width: 32  
			    root_folder: '/home/dennis/PycharmProjects/untitled/'  
			  }  
			}  		
			layer {  
			  name: 'conv1'  
			  type: 'Convolution' 
			  bottom: 'data' 
			  top: 'conv1' 
			  convolution_param {  
			    num_output: 20  
			    kernel_size: 5  
			    weight_filler { 
			      type: 'xavier' 
			    } 
			  } 
			} 
			layer { 
			  name: 'pool1' 
			  type: 'Pooling' 
			  bottom: 'conv1' 
			  top: 'pool1' 
			  pooling_param { 
			    pool: MAX 
			    kernel_size: 2 
			    stride: 2 
			  } 
			} 
			layer { 
			  name: 'conv2' 
			  type: 'Convolution' 
			  bottom: 'pool1' 
			  top: 'conv2' 
			  convolution_param { 
			    num_output: 50 
			    kernel_size: 5 
			    weight_filler { 
			      type: 'xavier' 
			    } 
			  } 
			} 
			layer { 
			  name: 'pool2' 
			  type: 'Pooling' 
			  bottom: 'conv2' 
			  top: 'pool2' 
			  pooling_param { 
			    pool: MAX 
			    kernel_size: 2 
			    stride: 2 
			  } 
			} 
			layer { 
			  name: 'ip1' 
			  type: 'InnerProduct' 
			  bottom: 'pool2' 
			  top: 'ip1' 
			  inner_product_param { 
			    num_output: 500 
			    weight_filler { 
			      type: 'xavier' 
			    } 
			  } 
			} 
			layer { 
			  name: 'relu1' 
			  type: 'ReLU' 
			  bottom: 'ip1' 
			  top: 'ip1' 
			} 
			layer { 
			  name: 'ip2' 
			  type: 'InnerProduct' 
			  bottom: 'ip1' 
			  top: 'ip2' 
			  inner_product_param { 
			    num_output: 62 
			    weight_filler { 
			      type: 'xavier' 
			    } 
			  } 
			} 
			layer { 
			  name: 'loss' 
			  type: 'SoftmaxWithLoss' 
			  bottom: 'ip2' 
			  bottom: 'label' 
			  top: 'loss' 
			  include {  
			    phase: TRAIN  
			  }  
			}
			layer { 
			  name: 'prob' 
			  type: 'Softmax' 
			  bottom: 'ip2' 			  
			  top: 'prob' 
			  include {  
			    phase: TEST  
			  }  
			}
";
		string trainingsAndTestNetWithSolverDefinition = 
			@"test_iter: 100 
			test_interval: 500 
			base_lr: 0.01 
			momentum: 0.9 
			weight_decay: 0.0005 
			lr_policy: 'inv' 
			gamma: 0.0001 
			power: 0.75 
			display: 5 
			max_iter: 100 
			snapshot: 1000 
			snapshot_prefix: 'character' 
			net_param {{ 
			{0}
			}}";

		[Test ()]
		public void ShouldBeAbleToCreateTrainingInstance ()
		{			
			IntPtr instance = Wrapper.CreateTrainingInstance ();

			instance.ShouldNotEqual (IntPtr.Zero);

			var net = string.Format (trainingsAndTestNetWithSolverDefinition, netDefinition);

			var trainedNetAsString = Wrapper.Train (instance, net);

			Wrapper.ReleaseInstance (ref instance);

			instance.ShouldEqual (IntPtr.Zero);
			trainedNetAsString.Length.ShouldBeGreaterThan (0);
		}

		[Test ()]
		public void ShouldBeAbleToCreateTestInstance ()
		{
			var trainedNet = File.ReadAllText ("trainedNet.nettra");

			const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var letters = LETTERS.ToCharArray ().Select (l => l.ToString ()).ToArray ();
			var instance = Wrapper.CreateClassifyingInstance (netDefinition, trainedNet, "C#", letters, 62);

			instance.ShouldNotEqual (IntPtr.Zero);

			var results = new List<string> ();
			unsafe {
				IntPtr[] result = new IntPtr[5];
				Wrapper.Classify (instance, "/home/dennis/PycharmProjects/untitled/arial-f.jpg", result, 5);	

				foreach (var r in result) {
					results.Add (Marshal.PtrToStringAuto (r));
				}
			}

			results.First ().ShouldEqual ("f");

			Wrapper.ReleaseInstance (ref instance);

			instance.ShouldEqual (IntPtr.Zero);
		}
	}
}

