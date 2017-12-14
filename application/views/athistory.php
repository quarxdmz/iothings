                        <div class="row">
                            <div class="col-lg-6">
                                <div class="card">
                                    <div class="card-header">
                                        <i class="fa fa-align-justify"></i> Departures <span class="badge badge-primary">GMT+8</span>
                                    </div>
                                    <div class="card-body">
                                        <table class="table table-striped">
						    <thead>
						        <tr>
						            <th>Plane Name</th>
						            <th>Destination</th>
						            <th>Departure</th>
						        </tr>
						    </thead>
						    <tbody>
						        <?php foreach ($departures as $dData) : ?>
						        <tr>
						            <td><?=$dData->hex?></td>
						            <td><?=$dData->origin?></td>
						            <td><?=$dData->departure?></td>
						        </tr>
						        <?php endforeach;?>
						    </tbody>
						</table>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="card">
                                    <div class="card-header">
                                        <i class="fa fa-align-justify"></i> Arrivals <span class="badge badge-primary">GMT+8</span>
                                    </div>
                                    <div class="card-body">
                                         <table class="table table-striped">
						    <thead>
						        <tr>
						            <th>Plane Name</th>
						            <th>Origin</th>
						            <th>Arrival</th>
						        </tr>
						    </thead>
						    <tbody>        
						       <?php foreach ($arrivals as $aData) : ?>
						        <tr>
						            <td><?=$aData->hex?></td>
						            <td><?=$aData->origin?></td>
						            <td><?=$aData->arrival?></td>
						        </tr>
						        <?php endforeach;?>        
						    </tbody>
						</table>
                                    </div>
                                </div>
                            </div>
                        </div>