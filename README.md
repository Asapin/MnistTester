# Mnist Classifier Tester
Small console app for testing how good your MNIST model is recognizing which digit you wrote.

# How to use
* [Install CNTK 2.4.0 on your machine](https://docs.microsoft.com/en-us/cognitive-toolkit/setup-cntk-on-your-machine)
* Draw some digit in "mnist_digit.png"
* Place your model in "Model" directory or use included one
* Run the app

If you choose to use your own model, be sure that your model has 784 neurons as input (with dimensions either {784 x 1 x 1} or {28 x 28 x 1}) and 10 neurons as output.
