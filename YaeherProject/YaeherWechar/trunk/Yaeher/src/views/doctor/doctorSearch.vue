<template>
    <div class="doctor-search padding-top">
        <mt-header fixed title="搜索结果">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字" />
                    <div @click="searchDoctor">搜索</div>
                </div>
            </div>
            <ul class="group1-list">
                <li v-for="(item, index) in recordList" :key="index" class="mui-table-view-cell">
			        <router-link :to="{path: '/order-look', query: {id: item.id}}" class="mui-navigate-right">
			        	<div class="recordCase">
			        		<div class="recordTop">
			        			<div class="recordLeft">
				        			<div class="recordTitle">
										<span v-if="item.consultType === 'ImageText'">文</span>
										<span v-if="item.consultType === 'Phone'">电</span>
				        				<p class="font-gray">{{item.patientName}} {{item.sex | sexSelect}} {{item.age}} {{item.iiInessType}}</p>
				        				<p class="font-overflow">{{item.iiInessDescription}}</p>
				        			</div>
			        			</div>
			        			<div class="recordRight">
									<div v-if="item.consultStateCode!=statusSuccess" class="red-hint hint">{{item.consultState}}</div>
									<div v-if="item.consultStateCode===statusSuccess && !item.isReturnVisit"  class="red-hint hint">未回访</div>
									<div v-if="item.consultStateCode===statusSuccess && item.isReturnVisit"  class="green-hint hint">已回访</div>
									<div v-if="item.consultStateCode===statusSuccess && !item.isEvaluate"  class="red-hint hint">未评价</div>
									<div v-if="item.consultStateCode===statusSuccess && item.isEvaluate"  class="green-hint hint">已评价</div>
			        			</div>
			        			
			        		</div>
			        		<div class="recordBottom">
			        			<div class="userTime">
			        				<p>剩余{{item.hasInquiryTimes}}次追问</p>
			        				<p class="text-right">{{item.createdOn}}</p>
			        			</div>
			        		</div>
			        	</div>
			        </router-link>
			    </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import Star from 'components/star/star'
let moment = require('moment');
export default {
    components: {
      Star
    },
    data () {
        return {
            keyWord: '',
            doctorState: 1,
            recordList: [],
            createdBy: 0,
            statusSuccess: 'success',
        }
    },
    filters: {
        sexSelect (value) {
            const sexStatus = ['', '男', '女']
            return sexStatus[value]
        }
    },
    methods: {
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchDoctor()
            } 
        },
        searchDoctor() { // 搜索咨询
            const keyWord = this.keyWord
            if (!keyWord) {
                Toast('请输入搜索内容')
                return
            }
            this.$refs.inputText.blur()
            const createdBy = this.createdBy
            if (createdBy) {
                this.instance.consultationPageD({
                    keyWord,
                    createdBy
                })
                    .then((response) => {
                        this.recordList = response.data.result.item.items
                        for (let i = 0; i < this.recordList.length; i++) {
                            this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        }
                    })
                    .catch((error) => {
                    })
            } else {
                this.instance.consultationPageD({
                    keyWord
                })
                    .then((response) => {
                        this.recordList = response.data.result.item.items
                        for (let i = 0; i < this.recordList.length; i++) {
                            this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        }
                    })
                    .catch((error) => {
                    })
            }
            
        }
    },
    mounted () {
        this.searchDoctor()
    },
    created () {
        this.keyWord = this.$route.query.keyWord
        this.createdBy = parseInt(this.$route.query.createdBy)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctor-search {
    // .searchCase {
    //     padding: 0 10px 10px;
    //     background-color: $default-color; 
    //     .searchBox {
    //         background: $color-wfont url(../../assets/image/search-pic.png) 8px 12px no-repeat; 
    //         padding: 5px 15px 10px 23px;
    //         background-size: 14px 14px;
    //         display: flex;
    //         line-height: 19px;
    //         input {
    //             border: none;
    //             outline: none;
    //             flex: 1;
    //             font-size: $font-l;
    //         }
    //         div {
    //             width: 50px;
    //             text-align: right;
    //             font-size: $font-l;
    //             color: $color-bfont;
    //             line-height: 20px;
    //         }
    //     }
    // }

    .recordCase {padding: 0px 10px; font-size: $font-m;}
	.recordList {margin: 10px 0 0;}
	.recordList .mui-table-view-cell:after {height: 5px;}
	.recordTop {display: flex; padding-bottom: 0px;}
	.font-gray {color: $color-bfont;}
	.recordRight {width: 50px; color: $color-green; text-align: right;}
	.recordRight .hint {padding-bottom: 3px;}
	.recordLeft {flex: 1; display: flex;}
	.recordAvatar {width: 70px;}
	.recordTitle {flex: 1; text-align: left; position: relative;}
	.recordTitle span { width: 18px; height: 18px; color: $color-wfont; background-color: $color-red; position: absolute; top:-2px; left: -21px; font-size: $font-m;
	display: inline-block; border-radius: 9px; line-height: 19px; text-align: center;}
	.recordBottom {padding-top: 1px;}
	.userTime {display: flex;}
	.userTime p {flex: 1; font-size: $font-m; color: $color-bfont}

    .font-overflow { 
        width: 250px; 
        margin: 5px 0; 
        font-size: $font-l; 
        white-space: nowrap; 
        overflow: hidden; 
        text-overflow: ellipsis;
    }
}


</style>