using NUnit.Framework;
using System;
using Caffe;
using Should;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CaffeTests
{
	[TestFixture ()]
	public class CaffeTest
	{
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
			net_param { 
			name: 'TestNetwork'
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
			  name: 'data'  
			  type: 'ImageData'  
			  top: 'data'  
			  top: 'label'  
			  include {  
			    phase: TEST  
			  }  
			  transform_param {  
			    scale: 0.00392156862745  
			    mirror: true  
			    crop_size: 32  
			  }  
			  image_data_param {  
			    source: '/home/dennis/PycharmProjects/untitled/images.txt'  
			    batch_size: 1  
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
			}
			}";

		[Test ()]
		public void ShouldBeAbleToCreateTrainingInstance ()
		{			
			IntPtr instance = Wrapper.CreateTrainingInstance ();

			instance.ShouldNotEqual (IntPtr.Zero);

			var trainedNetAsString = Wrapper.Train (instance, trainingsAndTestNetWithSolverDefinition);

			Wrapper.ReleaseInstance (ref instance);

			instance.ShouldEqual (IntPtr.Zero);
			trainedNetAsString.Length.ShouldBeGreaterThan (0);
		}

		[Test ()]
		public void ShouldBeAbleToCreateTestInstance ()
		{
			var instance = Wrapper.CreateClassifyingInstance ("Hello", "from", "C#", 
				               new List<string> (){ "first", "second" }.ToArray (), 2);

			instance.ShouldNotEqual (IntPtr.Zero);

			Wrapper.ReleaseInstance (ref instance);

			instance.ShouldEqual (IntPtr.Zero);
		}
	}
}

