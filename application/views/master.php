<!DOCTYPE html>
<html lang="en">

    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <meta name="description" content="Cebu Airline Traffic, Realtime Airline Data of Incoming and Departing Planes, Historical Aircraft Data of Mactan International Airport">
        <meta name="author" content="IOThings.asia">
        <meta name="keyword" content="cebu airplane schedule, airplane schedule, airline, airline schedule, traffic, airline traffic, airline cebu, schedule, airplane, Mactan International Airport">
        <link rel="shortcut icon" href="<?php echo base_url(); ?>img/favicon.png">
        <title>Cebu-AirTraffic</title>
        <!-- Icons -->
        <link href="<?php echo base_url(); ?>node_modules/font-awesome/css/font-awesome.min.css" rel="stylesheet">
        <link href="<?php echo base_url(); ?>node_modules/simple-line-icons/css/simple-line-icons.css" rel="stylesheet">
        <!-- Main styles for this application -->
        <link href="<?php echo base_url(); ?>css/style.css" rel="stylesheet">
        <!-- Styles required by this views -->
    </head>
    <body class="app header-fixed sidebar-fixed aside-menu-fixed aside-menu-hidden">
        <?php echo $header; ?>
        <div class="app-body">
             <?php echo $sidebar; ?>
            <!-- Main content -->
            <main class="main">
                <!-- Breadcrumb -->
                <span class="breadcrumb">Current Time: <?php $date = new DateTime(null, new DateTimeZone('Asia/Manila'));
echo date_format($date, 'H:i');?></span>
                <!-- Breadcrumb Menu-->
                </ol>
                <div class="container-fluid">
                    <div class="animated fadeIn">
                             <?=$home?>
                    </div>
                </div>
                <!-- /.conainer-fluid -->
            </main>

        </div>
        <footer class="app-footer">
            <span>Cebu-AirTraffic</span>
            <span class="ml-auto">Powered by <a href="http://iothings.asia">IOThings.asia</a></span>
        </footer>
        <!-- Bootstrap and necessary plugins -->
        <script src="<?php echo base_url(); ?>node_modules/jquery/dist/jquery.min.js"></script>
        <script src="<?php echo base_url(); ?>node_modules/popper.js/dist/umd/popper.min.js"></script>
        <script src="<?php echo base_url(); ?>node_modules/bootstrap/dist/js/bootstrap.min.js"></script>
        <script src="<?php echo base_url(); ?>node_modules/pace-progress/pace.min.js"></script>
        <!-- Plugins and scripts required by all views -->
        <script src="<?php echo base_url(); ?>node_modules/chart.js/dist/Chart.min.js"></script>
        <!-- GenesisUI main scripts -->
        <script src="<?php echo base_url(); ?>js/app.js"></script>
        <!-- Plugins and scripts required by this views -->
        <!-- Custom scripts required by this view -->
        <script src="<?php echo base_url(); ?>js/views/main.js"></script>
    </body>

</html>